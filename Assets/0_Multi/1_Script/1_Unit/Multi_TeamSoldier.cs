using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class Multi_WeaponPoolManager : MonoBehaviourPun
{
    Queue<Multi_Projectile> weaponPool = new Queue<Multi_Projectile>();
    public Multi_Projectile[] SettingWeaponPool(GameObject weaponObj, int count)
    {
        Multi_Projectile[] _weapons = new Multi_Projectile[count];
        for (int i = 0; i < count; i++)
        {
            Multi_Projectile weapon = 
                PhotonNetwork.Instantiate(weaponObj.name, new Vector3(-500, -500, -500), weaponObj.transform.rotation).GetComponent<Multi_Projectile>();
            weapon.gameObject.SetActive(false);
            weaponPool.Enqueue(weapon);
            _weapons[i] = weapon;
        }
        return _weapons;
    }

    
    public Multi_Projectile UsedWeapon(Transform weaponPos, Vector3 dir, int speed, System.Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon = GetWeapon_FromPool();
        Vector3 pos = new Vector3(weaponPos.position.x, 2f, weaponPos.position.z);
        UseWeapon.Shot(pos, dir, speed, (Multi_Enemy enemy) => hitAction(enemy));
        return UseWeapon;
    }


    // 풀에서 잠깐 꺼내고 다시 들어감
    public Multi_Projectile GetWeapon_FromPool()
    {
        Multi_Projectile getWeapon = weaponPool.Dequeue();
        getWeapon.myRPC.RPC_Active(true);
        StartCoroutine(Co_ReturnWeapon_ToPool(getWeapon, 5f));
        return getWeapon;
    }

    IEnumerator Co_ReturnWeapon_ToPool(Multi_Projectile _weapon, float time)
    {
        yield return new WaitForSeconds(time);
        _weapon.myRPC.RPC_Active(false);
        _weapon.transform.position = new Vector3(-500, -500, -500);
        weaponPool.Enqueue(_weapon);
    }
}

public class Multi_TeamSoldier : MonoBehaviourPun, IPunObservable
{
    public UnitClass unitClass;
    public UnitColor unitColor;

    public int originDamage;
    public int originBossDamage;
    public float originAttackDelayTime;

    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public int damage;
    public int bossDamage;
    public int skillDamage;
    public int ApplySkillDamage => skillDamage;
    public float stopDistanc;

    // 상태 변수(동기화되지 않음)
    public bool isAttack; // 공격 중에 true
    public bool isAttackDelayTime; // 공격 쿨타임 중에 true
    // 나중에 유닛별 공격 조건 만들면서 없애기
    public bool isSkillAttack; // 스킬 공격 중에 true

    public Transform target;
    protected Multi_Enemy TargetEnemy { get { return target.GetComponent<Multi_Enemy>(); } }

    protected Multi_WeaponPoolManager poolManager = null;
    protected NavMeshAgent nav;
    protected Animator animator;
    protected PhotonView pv;
    protected AudioSource unitAudioSource;
    [SerializeField] protected AudioClip normalAttackClip;
    public float normalAttakc_AudioDelay;

    public GameObject reinforceEffect;
    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리

    // 적에게 대미지 입히기, 패시브 적용 등의 역할을 하는 델리게이트
    public delegate void Delegate_OnHit(Multi_Enemy enemy);
    protected Delegate_OnHit delegate_OnHit; // 평타
    protected Delegate_OnHit delegate_OnSkile; // 스킬
    public event Delegate_OnHit delegate_OnPassive; // 패시브

    private void Awake()
    {
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 40;

        // 아래에서 평타랑 스킬 설정할 때 delegate_OnPassive가 null이면 에러가 떠서 에러 방지용으로 실행 후에 OnEnable에서 덮어쓰기 때문에 의미 없음
        SetPassive();

        // 평타 설정
        delegate_OnHit += AttackEnemy;
        delegate_OnHit += delegate_OnPassive;
        // 스킬 설정
        skillDamage = 150; // 테스트 코드
        delegate_OnSkile += (Multi_Enemy enemy) => enemy.photonView.RPC("OnDamage", RpcTarget.MasterClient, ApplySkillDamage);
        delegate_OnSkile += delegate_OnPassive;

        // 유니티에서 class는 게임오브젝트의 컴포넌트로서만 작동하기 때문에 컴포넌트로 추가 후 사용해야한다.(폴더 내에 C#스크립트 생성 안해도 됨)
        // Unity초보자가 많이 하는 실수^^
        gameObject.AddComponent<Multi_WeaponPoolManager>();
        poolManager = GetComponent<Multi_WeaponPoolManager>();

        // 변수 선언
        //enemySpawn = FindObjectOfType<EnemySpawn>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();
        unitAudioSource = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();

        chaseRange = 150f;
        enemyDistance = 150f;
        nav.speed = this.speed;

        // 유닛별 세팅
        OnAwake();
    }
    public virtual void OnAwake() { }

    protected void SetPoolObj(GameObject _obj, int _count)
    {
        if(pv.IsMine) poolManager.SettingWeaponPool(_obj, _count);
    }

    void OnEnable()
    {
        //SetData();
        SetPassive();
        //UnitManager.instance.AddCurrentUnit(this);

        if (animator != null) animator.enabled = true;
        nav.enabled = true;

        // 적 추적
        UpdateTarget();
        if(pv.IsMine) StartCoroutine("NavCoroutine");
    }

    void SetData()
    {
        //UnitManager.instance.ApplyUnitData(gameObject.tag, this);
        SetInherenceData();
    }
    // 기본 데이터를 기반으로 유닛 고유 데이터 세팅
    public virtual void SetInherenceData() { }

    void SetPassive()
    {
        Multi_UnitPassive _passive = GetComponent<Multi_UnitPassive>();
        if (_passive == null) return;
        if (delegate_OnPassive != null) delegate_OnPassive = null;
        //UnitManager.instance.ApplyPassiveData(gameObject.tag, _passive, unitColor);
        _passive.SetPassive(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetChaseSetting(null);
        rayHitTransform = null;
        //UnitManager.instance.RemvoeCurrentUnit(this);
        isAttack = false;
        isAttackDelayTime = false;
        isSkillAttack = false;
        contactEnemy = false;
        enemyIsForward = false;
        enemyDistance = 1000f;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0);
            animator.enabled = false;
        }
        nav.enabled = false;
    }


    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        // currnetEnemyList에서 가져온 가장 가까운 enemy의 정보를 가지고 nav 및 변수 설정
        GameObject targetObject = GetProximateEnemy_AtList(Multi_EnemySpawner.instance.currentNormalEnemyList);
        SetChaseSetting(targetObject);
    }

    public void SetChaseSetting(GameObject targetObject) // 추적 관련 변수 설정
    {
        if (targetObject != null)
        {
            nav.isStopped = false;
            target = targetObject.transform;
            layerMask = ReturnLayerMask(target.gameObject);
        }
        else
        {
            nav.isStopped = true;
            target = null;
        }
    }
    // Proximate : 가장 가까운
    protected GameObject GetProximateEnemy_AtList(List<GameObject> _list)
    {
        float shortDistance = chaseRange;
        GameObject returnObject = null;
        if (_list.Count > 0)
        {
            foreach (GameObject enemyObject in _list)
            {
                if (enemyObject != null)
                {
                    float distanceToEnemy = Vector3.Distance(this.transform.position, enemyObject.transform.position);
                    if (distanceToEnemy < shortDistance)
                    {
                        shortDistance = distanceToEnemy;
                        returnObject = enemyObject;
                    }
                }
            }
        }
        return returnObject;
    }


    public virtual Vector3 DestinationPos { get; set; }
    protected bool contactEnemy = false;
    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴(로컬에서만 돌아감)
    {
        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, target.position);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            nav.SetDestination(DestinationPos);

            if ((enemyIsForward || contactEnemy) && !isAttackDelayTime && !isSkillAttack && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
            {
                //Debug.Log("Attack Start!!!");
                UnitAttack();
            }
            yield return null;
        }
    }

    [SerializeField] private int specialAttackPercent;
    [PunRPC] // 마스터 클라이언트에서만 실행
    public void UnitAttack()
    {
        // RPC딜레이 때문에 여기서 상태 변수 바꿈
        isAttack = true;
        isAttackDelayTime = true;
        pv.RPC("AttackFromHost", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void AttackFromHost()
    {
        int random = Random.Range(1, 101);
        bool isNormal = (random >= specialAttackPercent);
        photonView.RPC("SelectAttack", RpcTarget.All, isNormal);
    }

    [PunRPC] 
    public void SelectAttack(bool _isNormal)
    {
        if (_isNormal) NormalAttack();
        else SpecialAttack();
    }

    // 유닛들의 고유한 공격을 구현하는 가상 함수
    public virtual void NormalAttack() {}

    // 유닛마다 다른 스킬공격 (기사, 법사는 없음)
    public virtual void SpecialAttack() {}

    public void EndAttack()
    {
        if (!pv.IsMine) return;

        StartCoroutine(Co_ResetAttactStatus());
        if (target != null && TargetIsNormalEnemy && enemyDistance > stopDistanc * 2) UpdateTarget();
    }

    IEnumerator Co_ResetAttactStatus()
    {
        isAttack = false;

        yield return new WaitForSeconds(attackDelayTime);
        isAttackDelayTime = false;
    }

    protected void StartAttack()
    {
        StartCoroutine(Co_NormalAttackClipPlay());
    }

    IEnumerator Co_NormalAttackClipPlay()
    {
        yield return new WaitForSeconds(normalAttakc_AudioDelay);
        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);
    }


    protected int layerMask; // Ray 감지용
    [SerializeField]
    protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;

    public bool enemyIsForward;

    private void Update()
    {
        if (target == null) return;

        UnitTypeMove();
        enemyIsForward = Set_EnemyIsForword();
    }

    public virtual void UnitTypeMove() { } // 유닛에 따른 이동
    public Transform rayHitTransform;
    bool Set_EnemyIsForword()
    {
        if (rayHit)
        {
            rayHitTransform = rayHitObject.transform;
            if (rayHitTransform == null) return false;

            if (CheckObjectIsBoss(rayHitTransform.gameObject) || rayHitTransform == target) return true;
            else if (ReturnLayerMask(rayHitTransform.gameObject) == layerMask)
            {
                // ray에 맞은 적이 target은 아니지만 target과 같은 layer라면 두 enemy가 겹친 것으로 판단해 true를 리턴
                return true;
            }
        }

        return false;
    }

    bool CheckObjectIsBoss(GameObject enemy) // target을 사용하는게 아니라 킹쩔 수 없음
    {
        return enemy.CompareTag("Tower") || enemy.CompareTag("Boss");
    }

    int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
    {
        int layer = targetObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        return 1 << LayerMask.NameToLayer(layerName);
    }





    // 타워 때리는 무한반복 코루틴
    IEnumerator TowerNavCoroutine()
    {
        Physics.Raycast(transform.position + Vector3.up, target.position - transform.position, out RaycastHit towerHit, 100f, layerMask);

        Invoke("RangeNavStop", 3f); // 원거리 타워에 다가가는거 막기
        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, towerHit.point);
            if (target == null || enemyDistance > chaseRange)
            {
                Multi_EnemyTower currentTower = Multi_EnemySpawner.instance.CurrentTower;
                yield return new WaitUntil(() => currentTower != null);
                if (currentTower.isRespawn) SetChaseSetting(currentTower.gameObject);
                else
                {
                    yield return null;
                    continue;
                }
            }

            nav.SetDestination(towerHit.point);
            enemyDistance = Vector3.Distance(this.transform.position, towerHit.point);

            if ((contactEnemy || enemyIsForward) && !isAttackDelayTime && !isSkillAttack && !isAttack)
                UnitAttack();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void RangeNavStop()
    {
        if (GetComponent<RangeUnit>() != null)
        {
            nav.isStopped = true;
            nav.speed = 0f;
        }
    }

    //protected bool towerEnter; // 타워 충돌감지
    public bool enterStoryWorld;

    public void Unit_WorldChange()
    {
        StopCoroutine("Unit_WorldChange_Coroutine");
        StartCoroutine("Unit_WorldChange_Coroutine");
    }

    IEnumerator Unit_WorldChange_Coroutine() // 월드 바꾸는 함수
    {
        yield return new WaitUntil(() => !isAttack);
        nav.enabled = false;
        UnitManager.instance.ShowTpEffect(transform);

        if (!enterStoryWorld)
        {
            // 적군의 성 때 겹치는 버그 방지
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            transform.position = UnitManager.instance.Set_StroyModePosition();
            StopCoroutine("NavCoroutine");
            SetChaseSetting(EnemySpawn.instance.currentTower.gameObject);
            StartCoroutine("TowerNavCoroutine");
        }
        else
        {
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            transform.position = SetRandomPosition(20, -20, 10, -10, false);
            StopCoroutine("TowerNavCoroutine");
            UpdateTarget();
            StartCoroutine("NavCoroutine");
        }

        nav.enabled = true;
        enterStoryWorld = !enterStoryWorld;
        SoundManager.instance.PlayEffectSound_ByName("TP_Unit");
    }

    Vector3 SetRandomPosition(float maxX, float minX, float maxZ, float minZ, bool isTower)
    {
        float randomX;
        if (isTower)
        {
            float randomX_1 = Random.Range(minX, 480);
            float randomX_2 = Random.Range(520, maxX);
            int xArea = Random.Range(0, 2);
            randomX = (xArea == 0) ? randomX_1 : randomX_2;
        }
        else randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, 0, randomZ);
    }

    // 현재 타겟이 노말인지 아닌지 나타내는 프로퍼티
    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<Multi_Enemy>().enemyType == EnemyType.Normal); } }

    void AttackEnemy(Multi_Enemy enemy) // Boss랑 쫄병 구분해서 대미지 적용
    {
        damage = 100; // 테스트를 위한 임시 코드
        if (TargetIsNormalEnemy) enemy.photonView.RPC("OnDamage", RpcTarget.MasterClient, damage);
        else enemy.OnDamage(bossDamage);
    }


    // 동기화
    Vector3 currentPos;
    Quaternion currentDir;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
            currentDir = (Quaternion)stream.ReceiveNext();
            transform.position = currentPos;
            transform.rotation = currentDir;
        }
    }
}
