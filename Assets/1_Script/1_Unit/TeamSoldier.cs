using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponPoolManager : MonoBehaviour
{
    Queue<CollisionWeapon> weaponPool = new Queue<CollisionWeapon>();
    public void SettingWeaponPool(GameObject weaponObj, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CollisionWeapon weapon = Instantiate(weaponObj, new Vector3(-500, -500, -500), Quaternion.identity).GetComponent<CollisionWeapon>();
            weapon.gameObject.SetActive(false);
            weaponPool.Enqueue(weapon);
        }
    }

    public CollisionWeapon UsedWeapon(Transform weaponPos, Vector3 dir, int speed, System.Action<Enemy> hitAction)
    {
        CollisionWeapon UseWeapon = GetWeapon_FromPool();
        UseWeapon.transform.position = new Vector3(weaponPos.position.x, 2f, weaponPos.position.z);
        UseWeapon.Shoot(dir, speed, (Enemy enemy) => hitAction(enemy));
        return UseWeapon;
    }

    // 풀에서 잠깐 꺼내고 다시 들어감
    public CollisionWeapon GetWeapon_FromPool()
    {
        CollisionWeapon getWeapon = weaponPool.Dequeue();
        getWeapon.gameObject.SetActive(true);
        StartCoroutine(Co_ReturnWeapon_ToPool(getWeapon, 5f));
        return getWeapon;
    }

    IEnumerator Co_ReturnWeapon_ToPool(CollisionWeapon _weapon, float time)
    {
        yield return new WaitForSeconds(time);
        _weapon.gameObject.SetActive(false);
        _weapon.transform.position = new Vector3(-500, -500, -500);
        _weapon.transform.rotation = Quaternion.identity;
        weaponPool.Enqueue(_weapon);
    }
}

public class TeamSoldier : MonoBehaviour
{    
    public UnitClass unitClass;
    public UnitColor unitColor;

    // 아무 버프도 받지 않은 상태 변수 
    public int originDamage;
    public int originBossDamage;
    public float originAttackDelayTime;

    public float speed;
    public float attackDelayTime;
    public float attackRange;
    public int damage;
    public int bossDamage;
    public int skillDamage;
    public float stopDistanc;

    // 상태 변수
    public bool isAttack; // 공격 중에 true
    public bool isAttackDelayTime; // 공격 쿨타임 중에 true
    // 나중에 유닛별 공격 조건 만들면서 없애기
    public bool isSkillAttack; // 스킬 공격 중에 true

    protected NavMeshAgent nav;
    public Transform target;
    protected Enemy TargetEnemy { get { return target.GetComponent<Enemy>(); } }
    protected EnemySpawn enemySpawn;

    protected WeaponPoolManager poolManager = null;
    protected Animator animator;
    protected AudioSource unitAudioSource;
    [SerializeField] protected AudioClip normalAttackClip;
    public float normalAttakc_AudioDelay;

    public GameObject reinforceEffect;
    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리

    // 적에게 대미지 입히기, 패시브 적용 등의 역할을 하는 델리게이트
    public delegate void Delegate_OnHit(Enemy enemy);
    protected Delegate_OnHit delegate_OnHit; // 평타
    protected Delegate_OnHit delegate_OnSkile; // 스킬
    public event Delegate_OnHit delegate_OnPassive; // 패시브

    private void Awake()
    {
        // 아래에서 평타랑 스킬 설정할 때 delegate_OnPassive가 null이면 에러가 떠서 에러 방지용으로 실행 후에 OnEnable에서 덮어쓰기 때문에 의미 없음
        SetPassive();

        // 평타 설정
        delegate_OnHit += AttackEnemy;
        delegate_OnHit += delegate_OnPassive;
        // 스킬 설정
        delegate_OnSkile += (Enemy enemy) => enemy.OnDamage(skillDamage);
        delegate_OnSkile += delegate_OnPassive;

        // 유니티에서 class는 게임오브젝트의 컴포넌트로서만 작동하기 때문에 컴포넌트로 추가 후 사용해야한다.(폴더 내에 C#스크립트 생성 안해도 됨)
        // Unity초보자가 많이 하는 실수^^
        gameObject.AddComponent<WeaponPoolManager>();
        poolManager = GetComponent<WeaponPoolManager>();

        // 변수 선언
        enemySpawn = FindObjectOfType<EnemySpawn>();
        animator = GetComponent<Animator>();
        unitAudioSource = GetComponentInParent<AudioSource>();
        nav = GetComponentInParent<NavMeshAgent>();

        chaseRange = 150f;
        enemyDistance = 150f;
        nav.speed = this.speed;

        // 유닛별 세팅
        OnAwake();
    }
    public virtual void OnAwake() { }


    void OnEnable()
    {
        SetData();
        SetPassive();
        UnitManager.instance.AddCurrentUnit(this);

        if (animator != null) animator.enabled = true;
        nav.enabled = true;

        // 적 추적
        UpdateTarget();
        StartCoroutine("NavCoroutine");
    }

    void SetData()
    {
        UnitManager.instance.ApplyUnitData(gameObject.tag, this);
        SetInherenceData();
    }
    // 기본 데이터를 기반으로 유닛 고유 데이터 세팅
    public virtual void SetInherenceData() { }

    void SetPassive()
    {
        UnitPassive _passive = GetComponent<UnitPassive>();
        if (delegate_OnPassive != null) delegate_OnPassive = null;
        UnitManager.instance.ApplyPassiveData(gameObject.tag, _passive, unitColor);
        _passive.SetPassive(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetChaseSetting(null);
        rayHitTransform = null;
        UnitManager.instance.RemvoeCurrentUnit(this);
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



    public int specialAttackPercent;
    void UnitAttack()
    {
        int random = Random.Range(0, 100);
        if(random < specialAttackPercent) SpecialAttack();
        else
        {
            NormalAttack();
            PlayNormalAttackClip();
        }
    }

    protected void PlayNormalAttackClip()
    {
        StartCoroutine(Co_NormalAttackClipPlay());
    }

    IEnumerator Co_NormalAttackClipPlay()
    {
        yield return new WaitForSeconds(normalAttakc_AudioDelay);
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);
    }

    protected void StartAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
    }

    public virtual void NormalAttack()
    {
        // override 코루틴 마지막 부분에서 실행하는 코드
        StartCoroutine(Co_ResetAttactStatus());
        if (target != null && TargetIsNormalEnemy && enemyDistance > stopDistanc * 2) UpdateTarget();
    }

    IEnumerator Co_ResetAttactStatus()
    {
        isAttack = false;

        yield return new WaitForSeconds(attackDelayTime);
        isAttackDelayTime = false;
    }

    public virtual void SpecialAttack() { } // 유닛마다 다른 스킬공격 (기사, 법사는 없음)

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

    public virtual void UnitTypeMove() {} // 유닛에 따른 이동
    public Transform rayHitTransform;
    bool Set_EnemyIsForword()
    {
        if (rayHit)
        {
            rayHitTransform = rayHitObject.transform;
            if (rayHitTransform == null) return false;

            if (CheckObjectIsBoss(rayHitTransform.gameObject) || rayHitTransform == target.parent) return true;
            else if(ReturnLayerMask(rayHitTransform.GetChild(0).gameObject) == layerMask)
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


    public virtual Vector3 DestinationPos { get; set; }

    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴
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

            if ( (enemyIsForward || contactEnemy) && !isAttackDelayTime && !isSkillAttack && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
            {
                UnitAttack();
            }
            yield return null;
        }
    }
    protected bool contactEnemy = false;

    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (EnemySpawn.instance.BossRespawn) // 보스 있으면 보스가 타겟
        {
            SetChaseSetting(enemySpawn.currentBossList[0].gameObject);
            return;
        }

        // currnetEnemyList에서 가져온 가장 가까운 enemy의 정보를 가지고 nav 및 변수 설정
        GameObject targetObject = GetProximateEnemy_AtList(EnemySpawn.instance.currentEnemyList);
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



    // 타워 때리는 무한반복 코루틴
    IEnumerator TowerNavCoroutine()
    {
        Physics.Raycast(transform.parent.position + Vector3.up, target.position - transform.position, out RaycastHit towerHit, 100f, layerMask);

        Invoke("RangeNavStop", 3f); // 원거리 타워에 다가가는거 막기
        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, towerHit.point);
            if (target == null || enemyDistance > chaseRange)
            {
                yield return new WaitUntil(() => enemySpawn.currentTower != null);
                EnemyTower currentTower = enemySpawn.currentTower;
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
            transform.parent.position = UnitManager.instance.Set_StroyModePosition();
            StopCoroutine("NavCoroutine");
            SetChaseSetting(EnemySpawn.instance.currentTower.gameObject);
            StartCoroutine("TowerNavCoroutine");
        }
        else
        {
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            transform.parent.position = SetRandomPosition(20, -20, 10, -10, false);
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
    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<NomalEnemy>() ); } }

    void AttackEnemy(Enemy enemy) // Boss enemy랑 쫄병 구분
    {
        if (TargetIsNormalEnemy) enemy.OnDamage(damage);
        else enemy.OnDamage(bossDamage);
    }
}