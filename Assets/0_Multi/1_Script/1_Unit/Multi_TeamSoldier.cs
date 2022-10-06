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

    public Transform target;
    protected Multi_Enemy TargetEnemy { get { return target.GetComponent<Multi_Enemy>(); } }

    protected Multi_UnitPassive passive;
    protected NavMeshAgent nav;
    private ObstacleAvoidanceType originObstacleType;
    protected Animator animator;
    protected PhotonView pv;
    protected RPCable rpcable;
    [SerializeField] protected EffectSoundType normalAttackSound;
    public float normalAttakc_AudioDelay;

    protected Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;

    public event Action<Multi_TeamSoldier> OnDead;

    #region Virual Funtion
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void SetSkillDamage() { } // 기본 데이터를 기반으로 유닛 고유 데이터 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => _state.StartAttack();
    public virtual void UnitTypeMove() { } // 유닛에 따른 움직임
    #endregion

    [SerializeField] protected TargetManager _targetManager;
    [SerializeField] protected UnitState _state;
    public bool EnterStroyWorld => _state.EnterStroyWorld;

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
        enemyDistance = CHASE_RANGE;

        OnAwake(); // 유닛별 세팅

        _state = gameObject.AddComponent<UnitState>().SetInfo(rpcable);
        _targetManager = new TargetManager(_state);
        _targetManager.OnChangedTarget += SetNewTarget;
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
        _state.Daad();
    }

    void ResetAiStateValue()
    {
        _targetManager.Reset();
        target = null;
        rayHitTransform = null;
        contactEnemy = false;
        enemyIsForward = false;
        enemyDistance = CHASE_RANGE;

        if (animator != null)
            animator.enabled = false;

        nav.enabled = false;
    }

    void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        _targetManager.UpdateTarget(transform.position);
    }

    void TargetToBoss(Multi_BossEnemy boss) => _targetManager.ChangedTarget(boss);

    void SetNewTarget(Multi_Enemy newTarget)
    {
        if(newTarget == null)
        {
            target = null;
            nav.isStopped = true;
            return;
        }

        nav.isStopped = false;
        target = newTarget.transform;
        layerMask = ReturnLayerMask(target.gameObject);

        if (newTarget.enemyType == EnemyType.Tower) ChaseTower(newTarget);
    }

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
            if (target == null || Chaseable == false)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            nav.SetDestination(DestinationPos);

            if ((enemyIsForward || contactEnemy) && _state.IsAttackable) // && !isAttack && !isAttackDelayTime && !isSkillAttack
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
        _state.StartAttack();
        AfterPlaySound(normalAttackSound, normalAttakc_AudioDelay);
    }

    bool CheckTargetUpdateCondition => TargetIsNormalEnemy && enemyDistance > stopDistanc * 2 || (target != null && target.GetComponent<Multi_Enemy>().IsDead);
    protected void EndAttack()
    {
        _state.EndAttack(AttackDelayTime);
        if (CheckTargetUpdateCondition) UpdateTarget();
    }

    protected void EndSkillAttack(float coolTime)
    {
        _state.EndAttack(coolTime);
        if (CheckTargetUpdateCondition) UpdateTarget();
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
    readonly float CHASE_RANGE = 150f;
    protected bool Chaseable => CHASE_RANGE > enemyDistance; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기
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
        _state.ChangedWorld();
        if (EnterStroyWorld) EnterStroyMode();
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

        Vector3 GetOppositeWorldSpawnPos() => (EnterStroyWorld) ? Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(rpcable.UsingId) 
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
            => rpcable.UsingId == Multi_Managers.Camera.LookWorld_Id && EnterStroyWorld == Multi_Managers.Camera.IsLookEnemyTower;
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

    [Serializable]
    protected class UnitState : MonoBehaviour
    {
        public UnitState SetInfo(RPCable rpcable)
        {
            _rpcable = rpcable;
            return this;
        }

        public void Daad()
        {
            _enterStoryWorld = false;
            _isAttackable = true;
            _isAttack = false;
        }

        [SerializeField] bool _enterStoryWorld;
        public bool EnterStroyWorld => _enterStoryWorld;
        public void ChangedWorld()
        {
            _isAttackable = true;
            _isAttack = false;
            _enterStoryWorld = !_enterStoryWorld;
        }

        [SerializeField] bool _isAttackable = true;
        public bool IsAttackable => _isAttackable;

        [SerializeField] bool _isAttack;
        public bool IsAttack => _isAttack;

        public void StartAttack()
        {
            _isAttackable = false;
            _isAttack = true;
        }

        public void EndAttack(float coolTime)
        {
            _isAttack = false;
            StartCoroutine(Co_AttackCoolDown(coolTime));
        }

        IEnumerator Co_AttackCoolDown(float coolTime)
        {
            yield return new WaitForSeconds(coolTime);
            _isAttackable = true;
        }

        RPCable _rpcable;
        public int UsingId => _rpcable.UsingId;
    }

    [Serializable]
    protected class TargetManager
    {
        [SerializeField] Multi_Enemy _target;
        public Vector3 TargetPosition
        {
            get
            {
                if (_target == null) return Vector3.zero;
                else return _target.transform.position;
            }
        }
        public event Action<Multi_Enemy> OnChangedTarget;

        UnitState _state;
        public TargetManager(UnitState state) => _state = state;

        public void Reset() => _target = null;

        public void UpdateTarget(Vector3 position)
        {
            var newTarget = FindTarget(position);
            if (_target != newTarget) 
                ChangedTarget(newTarget);
        }

        Multi_Enemy FindTarget(Vector3 position)
        {
            if (_state.EnterStroyWorld) return Multi_EnemyManager.Instance.GetCurrnetTower(_state.UsingId);
            if (Multi_EnemyManager.Instance.GetCurrentBoss(_state.UsingId) != null) return Multi_EnemyManager.Instance.GetCurrentBoss(_state.UsingId);
            return Multi_EnemyManager.Instance.GetProximateEnemy(position, _state.UsingId);
        }

        public void ChangedTarget(Multi_Enemy newTarget)
        {
            _target = newTarget;
            OnChangedTarget?.Invoke(newTarget);
        }
    }

    [Serializable]
    protected class ChaseSystem
    {
        [SerializeField] bool isMoveLock;
        protected virtual bool IsMoveLock => false;

        NavMeshAgent _nav;
        Transform _currentTarget = null;
        public void ChangedTarget(Transform newTarget) => _currentTarget = newTarget;
        Transform _transform;
        Vector3 MyPosition => _transform.position;
        public ChaseSystem(NavMeshAgent nav, Transform transform)
        {
            _nav = nav;
            _transform = transform;
        }

        protected void LockMove()
        {
            if (_nav.updatePosition == false) return;
            _nav.updatePosition = false;
        }

        protected void ReleaseMove()
        {
            if (_nav.updatePosition == true) return;

            ResetNavPosition();
            _nav.updatePosition = true;
        }

        protected void ResetNavPosition()
        {
            _nav.Warp(MyPosition);
            if (_currentTarget != null)
                _nav.SetDestination(_currentTarget.position);
        }

        void FixedNavPosition()
        {
            if (Vector3.Distance(_nav.nextPosition, MyPosition) > 5f)
                ResetNavPosition();
        }

        public void MoveUpdate()
        {
            if (_currentTarget == null) return;

            isMoveLock = IsMoveLock;
            FixedNavPosition();
            // UnitTypeMove();
            enemyIsForward = Set_EnemyIsForword();
        }

        protected int layerMask; // Ray 감지용
        [SerializeField] protected float enemyDistance;
        readonly float CHASE_RANGE = 150f;
        protected bool Chaseable => CHASE_RANGE > enemyDistance; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기
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

                if (TransformIsBoss(rayHitTransform) || rayHitTransform == _currentTarget) return true;
                // TODO : 거리 체크하는 조건 추가하기
                else if (ReturnLayerMask(rayHitTransform.gameObject) == layerMask)
                {
                    // ray에 맞은 적이 target은 아니지만 target과 같은 layer라면 두 enemy가 겹친 것으로 판단해 true를 리턴
                    return true;
                }
            }

            return false;
        }
        bool TransformIsBoss(Transform enemy) => enemy.CompareTag("Tower") || enemy.CompareTag("Boss");

        int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
        {
            int layer = targetObject.layer;
            string layerName = LayerMask.LayerToName(layer);
            return 1 << LayerMask.NameToLayer(layerName);
        }
    }
}
