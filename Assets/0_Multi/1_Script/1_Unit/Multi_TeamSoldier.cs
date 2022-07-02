using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;
using Random = UnityEngine.Random;

public class Multi_TeamSoldier : MonoBehaviourPun, IPunObservable
{
    private UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    public UnitClass unitClass;
    public UnitColor unitColor;

    public int originDamage;
    public int originBossDamage;
    public float originAttackDelayTime;

    public float speed;
    
    public float attackRange;
    public int damage;
    public int bossDamage;
    public int skillDamage;
    public int ApplySkillDamage => skillDamage;
    public float stopDistanc;

    // 상태 변수(동기화되지 않음)
    public bool enterStoryWorld; // 적군의 성 입장시 true
    public bool isAttack; // 공격 중에 true
    public bool isAttackDelayTime; // 공격 쿨타임 중에 true
    public float attackDelayTime;
    // 나중에 유닛별 공격 조건 만들면서 없애기
    public bool isSkillAttack; // 스킬 공격 중에 true
    public float skillCoolDownTime;

    public Transform target;
    protected Multi_Enemy TargetEnemy { get { return target.GetComponent<Multi_Enemy>(); } }

    //protected Multi_WeaponPoolManager poolManager = null;
    protected NavMeshAgent nav;
    protected Animator animator;
    protected PhotonView pv;
    protected AudioSource unitAudioSource;
    [SerializeField] protected AudioClip normalAttackClip;
    public float normalAttakc_AudioDelay;

    public GameObject reinforceEffect;
    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리

    #region Events
    protected Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;
    protected Action<Multi_Enemy> OnSkileHit;

    public event Action<Multi_TeamSoldier> OnDead;
    #endregion

    #region Virual Funtion
    public virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void SetInherenceData() { } // 기본 데이터를 기반으로 유닛 고유 데이터 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => isSkillAttack = true; // 유닛마다 다른 스킬공격 (기사는 없음)
    public virtual void UnitTypeMove() { } // 유닛에 따른 움직임
    #endregion


    private void Awake()
    {
        // TODO : 아래 내용 공부하고 다른 쪽으로 빼기
        //PhotonNetwork.SendRate = 20;
        //PhotonNetwork.SerializationRate = 40;

        _unitFlags = new UnitFlags(unitColor, unitClass);

        SetPassive(); // 아래에서 평타랑 스킬 설정할 때 delegate_OnPassive가 null이면 에러가 떠서 에러 방지용으로 실행 후에 OnEnable에서 덮어쓰기 때문에 의미 없는 코드임

        // 평타 설정
        OnHit += AttackEnemy;
        OnHit += OnPassiveHit;
        // 스킬 설정
        skillDamage = 150; // 테스트 코드
        OnSkileHit += enemy => AttackEnemy(enemy, skillDamage);
        OnSkileHit += OnPassiveHit;

        // 변수 선언
        //enemySpawn = FindObjectOfType<EnemySpawn>();
        pv = GetComponent<PhotonView>();
        animator = GetComponentInChildren<Animator>();
        unitAudioSource = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();

        chaseRange = 150f;
        enemyDistance = 150f;
        nav.speed = this.speed;

        OnAwake(); // 유닛별 세팅
    }

    void Start()
    {
        Multi_SpawnManagers.BossEnemy.OnSpawn += _boss => SetChaseSetting(_boss.gameObject);
        Multi_SpawnManagers.BossEnemy.OnDead += _boss => UpdateTarget();
    }

    void OnEnable()
    {
        //SetData();
        SetPassive();

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

    void SetPassive()
    {
        Multi_UnitPassive _passive = GetComponent<Multi_UnitPassive>();
        if (_passive == null) return;
        if (OnPassiveHit != null) OnPassiveHit = null;
        //UnitManager.instance.ApplyPassiveData(gameObject.tag, _passive, unitColor);
        _passive.SetPassive(this);
    }

    public void Dead()
    {
        StopAllCoroutines();
        ResetValue();
        OnDead(this);
        gameObject.SetActive(false);
    }

    void ResetValue()
    {   
        SetChaseSetting(null);
        rayHitTransform = null;
        // TODO : OnDead로 event만들어서 스포너에서 구독하게 바꾸기
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

    // 현재 살아있는 enemy 중 가장 가까운 enemy의 정보를 가지고 nav 및 변수 설정
    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        //Transform _target = Multi_EnemyManager.Instance.GetProximateEnemy(transform.position, chaseRange);
        if (PhotonNetwork.IsMasterClient == false) return;
        Transform _target = Multi_EnemyManager.Instance.GetProximateEnemy(transform.position, chaseRange, GetComponent<Poolable>().UsingId);
        if (_target != null) SetChaseSetting(_target.gameObject);
        else SetChaseSetting(null);
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

    public virtual Vector3 DestinationPos { get; set; }
    protected bool contactEnemy = false;
    IEnumerator NavCoroutine() // 적을 추적하는 무한반복 코루틴(로컬에서만 돌아감)
    {
        if (PhotonNetwork.IsMasterClient == false) yield break;

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

    bool isRPC; // RPC딜레이 때문에 공격 2번 이상하는 버그 방지 변수
    [SerializeField] private int specialAttackPercent;
    void UnitAttack()
    {
        if (isRPC) return;
        isRPC = true;

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
        isRPC = false;
    }

    protected void StartAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        StartCoroutine(Co_NormalAttackClipPlay());
    }

    public void EndAttack()
    {
        // if (!pv.IsMine) return;

        StartCoroutine(Co_ResetAttactStatus());
        if (target != null && TargetIsNormalEnemy && enemyDistance > stopDistanc * 2) UpdateTarget();
    }

    IEnumerator Co_ResetAttactStatus()
    {
        isAttack = false;

        yield return new WaitForSeconds(attackDelayTime);
        isAttackDelayTime = false;
    }

    IEnumerator Co_NormalAttackClipPlay()
    {
        yield return new WaitForSeconds(normalAttakc_AudioDelay);
        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(normalAttackClip);
    }


    protected void SkillCoolDown(float _coolTime) => StartCoroutine(Co_SKillCoolDown(_coolTime));
    IEnumerator Co_SKillCoolDown(float _coolTime)
    {
        yield return new WaitForSeconds(_coolTime);
        isSkillAttack = false;
    }

    #region Enemy 추적
    private void Update()
    {
        if (target == null) return;

        UnitTypeMove();
        enemyIsForward = Set_EnemyIsForword();
    }

    protected int layerMask; // Ray 감지용
    [SerializeField] protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;
    public bool enemyIsForward;

    public Transform rayHitTransform;
    bool Set_EnemyIsForword()
    {
        if (rayHit)
        {
            rayHitTransform = rayHitObject.transform;
            if (rayHitTransform == null) return false;

            if (TransformIsBoss(rayHitTransform) || rayHitTransform == target) return true;
            // TODO : 거리 체크하는 조건 추가하기
            else if (ReturnLayerMask(rayHitTransform.gameObject) == layerMask)
            {
                // ray에 맞은 적이 target은 아니지만 target과 같은 layer라면 두 enemy가 겹친 것으로 판단해 true를 리턴
                return true;
            }
        }

        return false;
    }

    int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
    {
        int layer = targetObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        return 1 << LayerMask.NameToLayer(layerName);
    }
    #endregion


    #region enemy 관련 bool 값들
    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<Multi_Enemy>().enemyType == EnemyType.Normal); } }
    bool TransformIsBoss(Transform enemy) => enemy.CompareTag("Tower") || enemy.CompareTag("Boss");
    #endregion


    #region Enemy Tower
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
                // TOOD : 에너미 타워 구현하기
                Multi_EnemyTower currentTower = null; // Multi_EnemySpawner.instance.CurrentTower;
                yield return new WaitUntil(() => currentTower != null);
                if (!currentTower.isDead) SetChaseSetting(currentTower.gameObject);
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
            transform.position = Multi_WorldPosUtility.Instance.GetEnemyTower_TP_Position();
            StopCoroutine("NavCoroutine");
            SetChaseSetting(EnemySpawn.instance.currentTower.gameObject);
            StartCoroutine("TowerNavCoroutine");
        }
        else
        {
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            transform.position = Multi_WorldPosUtility.Instance.GetUnitSpawnPositon();
            StopCoroutine("TowerNavCoroutine");
            UpdateTarget();
            StartCoroutine("NavCoroutine");
        }

        nav.enabled = true;
        enterStoryWorld = !enterStoryWorld;
        SoundManager.instance.PlayEffectSound_ByName("TP_Unit");
    }
    #endregion


    #region callback funtion

    void AttackEnemy(Multi_Enemy enemy) // Boss랑 쫄병 구분해서 대미지 적용
    {
        if (TargetIsNormalEnemy) AttackEnemy(enemy, 100); // 100은 test용 원래는 damage 넣어야 됨
        else AttackEnemy(enemy, bossDamage);
    }

    void AttackEnemy(Multi_Enemy enemy, int damage) => enemy.OnDamage(damage);
    #endregion


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
            LerpUtility.LerpPostition(transform, currentPos);
            LerpUtility.LerpRotation(transform, currentDir);
            //transform.position = currentPos;
            //transform.rotation = currentDir;
        }
    }
}
