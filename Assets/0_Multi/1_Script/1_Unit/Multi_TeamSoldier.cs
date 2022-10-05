using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;

public class Multi_TeamSoldier : MonoBehaviourPun, IPunObservable
{
    private UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    public UnitClass unitClass;
    public UnitColor unitColor;

    [SerializeField] UnitStat stat;

    public int OriginDamage { get; private set; }
    public int OriginBossDamage { get; private set; }
    public float OriginAttackDelayTime { get; private set; }

    public int Damage { get => stat.Damage; set { stat.SetDamage(value); SetSkillDamage(); } }
    public int BossDamage { get => stat.BossDamage; set { stat.SetBossDamage(value); SetSkillDamage(); } }
    public float Speed { get => stat.Speed; set => stat.SetSpeed(value); }
    public float AttackDelayTime { get => stat.AttackDelayTime; set => stat.SetAttDelayTime(value); }
    public float AttackRange { get => stat.AttackRange; set => stat.SetAttackRange(value); }


    [SerializeField] protected float stopDistanc;

    protected bool enterStoryWorld;
    public bool EnterStroyWorld => enterStoryWorld;

    [SerializeField] protected bool isAttack; // 공격 중에 true
    [SerializeField] private bool isAttackDelayTime; // 공격 쿨타임 중에 true
    // 나중에 유닛별 공격 조건 만들면서 없애기
    [SerializeField] protected bool isSkillAttack; // 스킬 공격 중에 true
    [SerializeField] protected float skillCoolDownTime; // TODO : 죽이기
     
    public Transform target;
    protected Multi_Enemy TargetEnemy { get { return target.GetComponent<Multi_Enemy>(); } }

    protected Multi_UnitPassive passive;
    protected NavMeshAgent nav;
    private ObstacleAvoidanceType originObstacleType;
    protected Animator animator;
    protected PhotonView pv;
    protected RPCable rpcable;
    public int UsingId => rpcable.UsingId;
    [SerializeField] protected EffectSoundType normalAttackSound;
    public float normalAttakc_AudioDelay;

    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최대거리

    #region Events
    protected Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;

    public event Action<Multi_TeamSoldier> OnDead;
    #endregion

    #region Virual Funtion
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void SetSkillDamage() { } // 기본 데이터를 기반으로 유닛 고유 데이터 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => isSkillAttack = true; // 유닛마다 다른 스킬공격 (기사는 없음)
    public virtual void UnitTypeMove() { } // 유닛에 따른 움직임
    #endregion

    TargetManager _targetManager;
    UnitState _state;

    private void Awake()
    {
        _unitFlags = new UnitFlags(unitColor, unitClass);

        // 평타 설정
        OnHit += AttackEnemy;

        // 변수 선언
        passive = GetComponent<Multi_UnitPassive>();
        pv = GetComponent<PhotonView>();
        rpcable = GetComponent<RPCable>();
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();

        originObstacleType = nav.obstacleAvoidanceType;
        chaseRange = 150f;
        enemyDistance = 150f;

        OnAwake(); // 유닛별 세팅

        _state = new UnitState(rpcable);
        _targetManager = new TargetManager(_state);
        _targetManager.OnChangedTarget += SetNewDestinationPostion;
    }

    public void Spawn()
    {
        LoadStat_RPC();
        SetPassive_RPC();
        SetSpeed(Speed);
    }

    protected void SetSpeed(float speed) => nav.speed = speed;

    void OnEnable()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0);
        }

        nav.enabled = true;

        // 적 추적
        UpdateTarget();
        if (PhotonNetwork.IsMasterClient)
        {
            Multi_SpawnManagers.BossEnemy.OnSpawn -= TargetToBoss;
            Multi_SpawnManagers.BossEnemy.OnSpawn += TargetToBoss;
            StartCoroutine("NavCoroutine");
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetAiStateValue();
    }

    public void LoadStat_RPC() => pv.RPC("LoadStat", RpcTarget.All);
    [PunRPC]
    public void LoadStat()
    {
        stat = Multi_Managers.Data.GetUnitStat(UnitFlags);
        OriginDamage = stat.Damage;
        OriginBossDamage = stat.BossDamage;
        OriginAttackDelayTime = stat.AttackDelayTime;
        SetSkillDamage();
    }

    void SetPassive_RPC() => pv.RPC("SetPassive", RpcTarget.All);
    [PunRPC]
    public void SetPassive()
    {
        if (passive == null) return;

        if (OnPassiveHit != null)
        {
            OnHit -= OnPassiveHit;
            OnPassiveHit = null;
        }

        passive.LoadStat(UnitFlags);
        passive.SetPassive(this);

        if (OnPassiveHit != null)
            OnHit += OnPassiveHit;
    }

    public void Dead()
    {
        Debug.Assert(OnDead != null, $"{this.name} 이벤트가 null임");
        OnDead?.Invoke(this);
        gameObject.SetActive(false);
        Multi_SpawnManagers.BossEnemy.OnSpawn -= TargetToBoss;
        ResetSataeValue();
        _state.Reset();
    }

    void ResetSataeValue()
    {
        enterStoryWorld = false;
    }

    void ResetAiStateValue()
    {
        target = null;
        rayHitTransform = null;
        isAttack = false;
        isAttackDelayTime = false;
        isSkillAttack = false;
        contactEnemy = false;
        enemyIsForward = false;
        enemyDistance = 1000f;

        if (animator != null)
            animator.enabled = false;

        nav.enabled = false;
    }

    void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        _targetManager.UpdateTarget(transform.position);
        //ChangedTarget(FindTarget()); // TODO : State 관리하면서 바꾸기
    }

    Multi_Enemy FindTarget()
    {
        if (enterStoryWorld) return Multi_EnemyManager.Instance.GetCurrnetTower(UsingId);
        if (Multi_EnemyManager.Instance.GetCurrentBoss(UsingId) != null) return Multi_EnemyManager.Instance.GetCurrentBoss(UsingId);
        return Multi_EnemyManager.Instance._GetProximateEnemy(transform.position, chaseRange, UsingId);
    }

    void ChangedTarget(Multi_Enemy newTarget)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        _targetManager.ChangedTarget(newTarget);
    }

    void SetNewDestinationPostion(Multi_Enemy newTarget)
    {
        if(newTarget != null)
        {
            nav.isStopped = false;
            target = newTarget.transform;
            layerMask = ReturnLayerMask(target.gameObject);
        }

        if (newTarget.enemyType == EnemyType.Tower) ChaseTower(newTarget);
    }

    void TargetToBoss(Multi_BossEnemy boss) => _targetManager.ChangedTarget(boss);

    void ChaseTower(Multi_Enemy tower)
    {
        if (tower != null)
        {
            if (Physics.Raycast(transform.position, target.position - transform.position, out RaycastHit towerHit, 50f, layerMask))
                DestinationPos = towerHit.point;
            else
                DestinationPos = transform.position;
        }
    }


    protected virtual Vector3 DestinationPos { get; set; }
    protected bool contactEnemy = false;
    IEnumerator NavCoroutine()
    {
        if (PhotonNetwork.IsMasterClient == false) yield break;

        while (true)
        {
            if (target != null) enemyDistance = Vector3.Distance(this.transform.position, DestinationPos);
            if (target == null || enemyDistance > chaseRange)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            nav.SetDestination(DestinationPos);

            if ((enemyIsForward || contactEnemy) && !isAttackDelayTime && !isSkillAttack && !isAttack) // Attack가능하고 쿨타임이 아니면 공격
            {
                UnitAttack();
            }
            yield return null;
        }
    }

    bool isRPC; // RPC딜레이 때문에 공격 2번 이상하는 버그 방지 변수

    void UnitAttack()
    {
        if (isRPC) return;
        isRPC = true;
        pv.RPC("Attack", RpcTarget.All);
        isRPC = false;
    }

    [PunRPC]
    protected virtual void Attack() { }

    protected void StartAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        AfterPlaySound(normalAttackSound, normalAttakc_AudioDelay);
    }

    protected void EndAttack()
    {
        StartCoroutine(Co_ResetAttactStatus());
        if (TargetIsNormalEnemy && enemyDistance > stopDistanc * 2 || (target != null && target.GetComponent<Multi_Enemy>().IsDead))
            UpdateTarget();
    }

    IEnumerator Co_ResetAttactStatus()
    {
        isAttack = false;

        yield return new WaitForSeconds(AttackDelayTime);
        isAttackDelayTime = false;
    }


    protected void SkillCoolDown(float _coolTime) => StartCoroutine(Co_SKillCoolDown(_coolTime));
    IEnumerator Co_SKillCoolDown(float _coolTime)
    {
        yield return new WaitForSeconds(_coolTime);
        isSkillAttack = false;
    }

    #region Enemy 추적

    [SerializeField] bool isMoveLock;
    protected virtual bool IsMoveLock => false;

    protected void LockMove()
    {
        if (nav.updatePosition == false) return;
        nav.updatePosition = false;
    }

    protected void ReleaseMove()
    {
        if (nav.updatePosition == true) return;

        ResetNavPosition();
        nav.updatePosition = true;
    }

    protected void ResetNavPosition()
    {
        nav.Warp(transform.position);
        if(target != null)
            nav.SetDestination(target.position);
    }

    void FixedNavPosition()
    {
        if (Vector3.Distance(nav.nextPosition, transform.position) > 5f)
            ResetNavPosition();
    }

    private void Update()
    {
        if (target == null) return;

        isMoveLock = IsMoveLock;
        FixedNavPosition();
        UnitTypeMove();
        enemyIsForward = Set_EnemyIsForword();
    }

    protected int layerMask; // Ray 감지용
    [SerializeField] protected float enemyDistance;
    protected bool rayHit;
    protected RaycastHit rayHitObject;
    protected bool enemyIsForward;

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


    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<Multi_Enemy>().enemyType == EnemyType.Normal); } }
    bool TransformIsBoss(Transform enemy) => enemy.CompareTag("Tower") || enemy.CompareTag("Boss");

    public void ChagneWorld()
    {
        Multi_SpawnManagers.Effect.Play(Effects.Unit_Tp_Effect, transform.position + (Vector3.up * 3));
        
        MoveToOpposite();
        enterStoryWorld = !enterStoryWorld;
        _state.ChangedWorld();
        photonView.RPC("UpdateStatus", RpcTarget.All, enterStoryWorld);
        if (enterStoryWorld) EnterStroyMode();
        else EnterWolrd();

        UpdateTarget();
        Multi_Managers.Sound.PlayEffect(EffectSoundType.UnitTp);

        // 중첩 함수들...
        void MoveToOpposite()
        {
            rpcable.SetActive_RPC(false);
            rpcable.SetPosition_RPC(GetOppositeWorldSpawnPos());
            rpcable.SetActive_RPC(true);
        }

        Vector3 GetOppositeWorldSpawnPos() => (enterStoryWorld) ? Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(rpcable.UsingId) 
            : Multi_WorldPosUtility.Instance.GetEnemyTower_TP_Position(rpcable.UsingId);

        void EnterWolrd()
        {
            nav.obstacleAvoidanceType = originObstacleType;
        }

        void EnterStroyMode()
        {
            nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }

    [PunRPC]
    protected void UpdateStatus(bool isEnterStroyMode)
    {
        enterStoryWorld = isEnterStroyMode;
    }


    #region callback funtion

    void AttackEnemy(Multi_Enemy enemy) // Boss랑 쫄병 구분해서 대미지 적용
    {
        if (TargetIsNormalEnemy) AttackEnemy(enemy, Damage);
        else AttackEnemy(enemy, BossDamage);
    }

    void AttackEnemy(Multi_Enemy enemy, int damage, bool isSkill = false) => enemy.OnDamage(damage, isSkill);
    #endregion

    protected void SkillAttackToEnemy(Multi_Enemy enemy, int damage)
    {
        enemy.OnDamage(damage, isSkill: true);
        OnPassiveHit?.Invoke(enemy);
    }

    protected void AfterPlaySound(EffectSoundType type, float delayTime) => StartCoroutine(Co_AfterPlaySound(type, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType type, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlaySound(type);
    }
    protected void PlaySound(EffectSoundType type, float volumn = -1)
    {
        Multi_Managers.Sound.PlayEffect_If(type, SoundCondition, volumn);

        bool SoundCondition()
            => rpcable.UsingId == Multi_Managers.Camera.LookWorld_Id && enterStoryWorld == Multi_Managers.Camera.IsLookEnemyTower;
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
            LerpUtility.LerpPostition(transform, currentPos);
            LerpUtility.LerpRotation(transform, currentDir);
        }
    }

    class UnitState
    {
        public UnitState(RPCable rpcable)
        {
            _rpcable = rpcable;
        }

        public void Reset()
        {
            enterStoryWorld = false;
        }

        bool enterStoryWorld;
        public bool EnterStroyWorld => enterStoryWorld;
        public void ChangedWorld()
        {
            enterStoryWorld = !enterStoryWorld;
        }

        public bool isAttack; // 공격 중에 true
        public bool isAttackDelayTime; // 공격 쿨타임 중에 true

        RPCable _rpcable;
        public int UsingId => _rpcable.UsingId;
    }

    class TargetManager
    {
        Multi_Enemy _target;
        public Vector3 TargetPosition
        {
            get
            {
                if (_target == null) return Vector3.zero;
                else return _target.transform.position;
            }
        }
        public event Action<Multi_Enemy> OnChangedTarget;
        readonly float CHASE_RANGE = 150f;

        UnitState _state;

        public TargetManager(UnitState state)
        {
            _state = state;
        }

        public void Enter(Vector3 position)
        {
            UpdateTarget(position);
        }

        public void Reset()
        {
            _target = null;
        }

        public void UpdateTarget(Vector3 position)
        {
            var newTarget = FindTarget(position);
            if (newTarget != null && _target != newTarget)
                ChangedTarget(newTarget);
        }

        Multi_Enemy FindTarget(Vector3 position)
        {
            if (_state.EnterStroyWorld) return Multi_EnemyManager.Instance.GetCurrnetTower(_state.UsingId);
            if (Multi_EnemyManager.Instance.GetCurrentBoss(_state.UsingId) != null) return Multi_EnemyManager.Instance.GetCurrentBoss(_state.UsingId);
            return Multi_EnemyManager.Instance._GetProximateEnemy(position, CHASE_RANGE, _state.UsingId);
        }

        public void ChangedTarget(Multi_Enemy newTarget)
        {
            if (newTarget != null)
            {
                _target = newTarget;
                OnChangedTarget?.Invoke(newTarget);
            }
        }
    }
}
