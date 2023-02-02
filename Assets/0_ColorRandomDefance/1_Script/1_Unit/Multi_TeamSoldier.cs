using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;

public enum UnitColor { Red, Blue, Yellow, Green, Orange, Violet, White, Black };
public enum UnitClass { Swordman, Archer, Spearman, Mage }

public class Multi_TeamSoldier : MonoBehaviourPun //, IPunObservable
{
    private UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    public UnitClass unitClass;
    public UnitColor unitColor;

    [SerializeField] UnitStat stat;

    public int OriginDamage { get; private set; }
    public int OriginBossDamage { get; private set; }
    public float OriginAttackDelayTime { get; private set; }

    public int Damage { get => stat.Damage; set { stat.SetDamage(value); OnDamageChanaged?.Invoke(Damage); } }
    public int BossDamage { get => stat.BossDamage; set { stat.SetBossDamage(value); OnBossDamageChanged?.Invoke(BossDamage); } }
    public float Speed { get => stat.Speed; set => stat.SetSpeed(value); }
    public float AttackDelayTime { get => stat.AttackDelayTime; set => stat.SetAttDelayTime(value); }
    public float AttackRange { get => stat.AttackRange; set => stat.SetAttackRange(value); }

    public event Action<int> OnDamageChanaged = null;
    public event Action<int> OnBossDamageChanged = null;

    [SerializeField] protected float stopDistanc;

    public Transform target 
    {
        get
        {
            if (_targetManager.Target == null)
                return null;
            else
                return _targetManager.Target.transform;
        }
    }
    protected Multi_Enemy TargetEnemy => _targetManager.Target;

    protected Multi_UnitPassive passive;
    protected NavMeshAgent nav;
    private ObstacleAvoidanceType originObstacleType;
    protected Animator animator;
    protected PhotonView pv;
    protected RPCable rpcable;
    public int UsingID => rpcable.UsingId;
    [SerializeField] protected EffectSoundType normalAttackSound;
    public float normalAttakc_AudioDelay;

    protected Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;

    public event Action<Multi_TeamSoldier> OnDead;

    #region Virual Funtion
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => _state.StartAttack();
    #endregion

    [SerializeField] protected TargetManager _targetManager;
    protected UnitState _state;
    public bool EnterStroyWorld => _state.EnterStoryWorld;
    public bool IsAttack => _state.IsAttack;
    protected ChaseSystem _chaseSystem;

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

        _state = gameObject.AddComponent<UnitState>();
        _targetManager = new TargetManager(_state, transform);
        _targetManager.OnChangedTarget += SetNewTarget;
        _chaseSystem = AddCahseSystem();
        _targetManager.OnChangedTarget += _chaseSystem.ChangedTarget;
        OnAwake(); // 유닛별 세팅


        void SetNewTarget(Multi_Enemy newTarget)
        {
            if (gameObject.activeSelf == false) return;
            if (newTarget == null)
                nav.isStopped = true;
            else
                nav.isStopped = false;
        }
    }

    protected virtual ChaseSystem AddCahseSystem() => gameObject.AddComponent<ChaseSystem>();

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
            StartCoroutine(nameof(NavCoroutine));
        //Multi_SpawnManagers.BossEnemy.OnSpawn -= ChangeTargetToBoss;
        //Multi_SpawnManagers.BossEnemy.OnSpawn += ChangeTargetToBoss;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetAiStateValue();
    }

    void LoadStat_RPC() => pv.RPC(nameof(LoadStat), RpcTarget.All);
    [PunRPC]
    protected void LoadStat()
    {
        stat = Managers.Data.GetUnitStat(UnitFlags);
        OnDamageChanaged?.Invoke(Damage);
        OnBossDamageChanged?.Invoke(BossDamage);
        OriginDamage = stat.Damage;
        OriginBossDamage = stat.BossDamage;
        OriginAttackDelayTime = stat.AttackDelayTime;
    }
    
    void SetPassive_RPC() => pv.RPC(nameof(SetPassive), RpcTarget.All);
    [PunRPC]
    protected void SetPassive()
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
        OnDead?.Invoke(this);
        OnDead = null;
        gameObject.SetActive(false);
        Managers.Multi.Instantiater.PhotonDestroy(gameObject);
        _state.Dead();
    }

    void ResetAiStateValue()
    {
        _targetManager.Reset();
        contactEnemy = false;

        if (animator != null)
            animator.enabled = false;

        nav.enabled = false;
    }

    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        _targetManager.UpdateTarget();
    }

    public bool contactEnemy = false;
    IEnumerator NavCoroutine()
    {
        if (PhotonNetwork.IsMasterClient == false) yield break;

        while (true)
        {
            if (target == null || Chaseable == false)
            {
                UpdateTarget();
                yield return null; // 튕김 방지
                continue;
            }

            if ((_chaseSystem.EnemyIsForward || contactEnemy) && _state.IsAttackable)
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
        pv.RPC(nameof(Attack), RpcTarget.All);
        isRPC = false;
    }

    [PunRPC]
    protected virtual void Attack() { }

    protected void StartAttack()
    {
        _state.StartAttack();
        AfterPlaySound(normalAttackSound, normalAttakc_AudioDelay);
    }

    bool CheckTargetUpdateCondition => target == null || (TargetIsNormalEnemy && (enemyDistance > stopDistanc * 2 || target.GetComponent<Multi_Enemy>().IsDead));
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

    private void Update()
    {
        if (target == null) return;

        _chaseSystem.MoveUpdate();
    }

    [SerializeField] protected float enemyDistance => _chaseSystem.EnemyDistance;
    readonly float CHASE_RANGE = 150f;
    protected bool Chaseable => CHASE_RANGE > enemyDistance; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기

    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<Multi_Enemy>().enemyType == EnemyType.Normal); } }

    public void ChagneWorld()
    {
        Vector3 toPos = GetOppositeWorldSpawnPos();
        MoveToPos(toPos);
        photonView.RPC(nameof(MoveToPos), RpcTarget.Others, toPos);

        _state.ChangedWorld();
        if (EnterStroyWorld) EnterStroyMode();
        else EnterWolrd();

        UpdateTarget();
        RPC_PlayTpSound();


        void EnterWolrd() => nav.obstacleAvoidanceType = originObstacleType;
        void EnterStroyMode() => nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    [PunRPC]
    protected void MoveToPos(Vector3 pos)
    {
        Managers.Effect.PlayParticle("UnitTpEffect", transform.position + (Vector3.up * 3));
        gameObject.SetActive(false);
        transform.position = pos;
        gameObject.SetActive(true);
    }

    Vector3 GetOppositeWorldSpawnPos() => (EnterStroyWorld) ? Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(rpcable.UsingId)
            : Multi_WorldPosUtility.Instance.GetEnemyTower_TP_Position(rpcable.UsingId);

    void RPC_PlayTpSound() // 보는 쪽에서만 소리가 들려야 하므로 복잡해보이는 이 로직이 맞음.
    {
        if (_state.UsingId == Multi_Data.instance.Id)
            Managers.Sound.PlayEffect(EffectSoundType.UnitTp);
        else
            photonView.RPC(nameof(PlayTpSound), RpcTarget.Others);
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


    [PunRPC] // TODO : 나중에 멀티 싱글턴 만들어서 거기에 빼기
    protected void PlayTpSound() => Managers.Sound.PlayEffect(EffectSoundType.UnitTp);

    protected void AfterPlaySound(EffectSoundType type, float delayTime) => StartCoroutine(Co_AfterPlaySound(type, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType type, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlaySound(type);
    }
    protected void PlaySound(EffectSoundType type, float volumn = -1)
    {
        Managers.Sound.PlayEffect_If(type, SoundCondition, volumn);

        bool SoundCondition()
            => rpcable.UsingId == Managers.Camera.LookWorld_Id && EnterStroyWorld == Managers.Camera.IsLookEnemyTower;
    }

    [Serializable]
    protected class UnitState : MonoBehaviourPun
    {
        void Awake()
        {
            _rpcable = GetComponent<RPCable>();
        }

        public void Dead()
        {
            RPC_SetEnterStroyWorld(false);
            _isAttackable = true;
            _isAttack = false;
        }

        [SerializeField] bool _enterStoryWorld;
        public bool EnterStoryWorld => _enterStoryWorld;
        public void ChangedWorld()
        {
            _isAttackable = true;
            _isAttack = false;
            RPC_SetEnterStroyWorld(!_enterStoryWorld);
        }

        void RPC_SetEnterStroyWorld(bool newEnterStroyWorld) => photonView.RPC(nameof(SetEnterStroyWorld), RpcTarget.All, newEnterStroyWorld);
        [PunRPC]
        void SetEnterStroyWorld(bool newEnterStroyWorld) => _enterStoryWorld = newEnterStroyWorld;

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
        public Multi_Enemy Target => _target;
        public event Action<Multi_Enemy> OnChangedTarget;
        
        UnitState _state;
        Transform _transform;
        public TargetManager(UnitState state, Transform transform)
        {
            _state = state;
            _transform = transform;
        }

        public void Reset() => ChangedTarget(null);

        public void UpdateTarget()
        {
            var newTarget = FindTarget(_transform.position);
            if (_target != newTarget)
                ChangedTarget(newTarget);
        }

        Multi_Enemy FindTarget(Vector3 position)
        {
            if (_state.EnterStoryWorld) 
                return Multi_EnemyManager.Instance.GetCurrnetTower(_state.UsingId);
            if (Multi_EnemyManager.Instance.TryGetCurrentBoss(_state.UsingId, out Multi_BossEnemy boss)) 
                return boss;

            return Multi_EnemyManager.Instance.GetProximateEnemy(position, _state.UsingId);
        }

        void ChangedTarget(Multi_Enemy newTarget)
        {
            _target = newTarget;
            OnChangedTarget?.Invoke(newTarget);
            if(newTarget != null)
            {
                newTarget.OnDead -= ChangeTargetWhenTargetDead;
                newTarget.OnDead += ChangeTargetWhenTargetDead;
            }
        }

        void ChangeTargetWhenTargetDead(Multi_Enemy deadTarget)
        {
            Reset();
            UpdateTarget();
        }
    }
}
