using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;
using System.Linq;
using UnityEditor.Graphs;

public enum UnitColor { Red, Blue, Yellow, Green, Orange, Violet, White, Black };
public enum UnitClass { Swordman, Archer, Spearman, Mage }

public class Multi_TeamSoldier : MonoBehaviourPun
{
    [SerializeField] Unit _unit;
    public Unit Unit => _unit;
    public UnitFlags UnitFlags => _unit.UnitFlags;

    public ObjectSpot Spot { get; private set; }
    public bool IsInDefenseWorld => Spot.IsInDefenseWorld;

    public UnitClass UnitClass => UnitFlags.UnitClass;
    public UnitColor UnitColor => UnitFlags.UnitColor;

    [SerializeField] UnitStat _stat;

    protected int Damage => _unit.DamageInfo.ApplyDamage;
    protected int BossDamage => _unit.DamageInfo.ApplyBossDamage;
    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _unit.UpdateDamageInfo(newInfo);
    public float Speed => Unit.Stats.Speed; // { get => _stat.Speed; set => _stat.SetSpeed(value); }
    public float AttackDelayTime { get => Unit.Stats.AttackDelayTime; set => Unit.Stats.AttackDelayTime = value; }
    public float AttackRange => Unit.Stats.AttackRange;// _stat.AttackRange;

    [SerializeField] protected float stopDistanc;

    public Transform target => _targetManager.Target == null ? null : TargetEnemy.transform;
    protected Multi_Enemy TargetEnemy => _targetManager.Target;

    protected NavMeshAgent nav;
    private ObstacleAvoidanceType originObstacleType;
    protected Animator animator;
    protected RPCable rpcable;
    public byte UsingID => (byte)rpcable.UsingId;
    protected EffectSoundType normalAttackSound;
    public float normalAttakc_AudioDelay;

    public event Action<Multi_TeamSoldier> OnDead;

    // 가상 함수
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => _state.StartAttack();


    [SerializeField] readonly protected TargetManager _targetManager = new TargetManager();
    protected UnitState _state;
    public bool IsAttack => _state.UnitAttackState.IsAttack;
    protected ChaseSystem _chaseSystem;

    void Awake()
    {
        // 변수 선언
        rpcable = GetComponent<RPCable>();
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        originObstacleType = nav.obstacleAvoidanceType;
        worldAudioPlayer = gameObject.AddComponent<WorldAudioPlayer>();
        worldAudioPlayer.ReceiveInject(Managers.Camera, Managers.Sound);

        _state = gameObject.AddComponent<UnitState>();
    }

    void Start()
    {
        OnAwake(); // 유닛별 세팅
    }

    protected MonsterFinder TargetFinder { get; private set; }
    protected UnitAttacker UnitAttacker { get; private set; }
    //public void Injection(UnitFlags flag, UnitStat stat, UnitDamageInfo damInfo, MonsterManager monsterManager)
    //{
    //    TargetFinder = new MonsterFinder(monsterManager, UsingID);
    //    SetInfo(flag, stat, damInfo);
    //    ChaseTarget();
    //}

    public void Injection(Unit unit, MonsterManager monsterManager)
    {
        TargetFinder = new MonsterFinder(monsterManager, UsingID);
        _unit = unit;
        Spot = new ObjectSpot(UsingID, true);
        UnitAttacker = new UnitAttacker(_unit);
        photonView.RPC(nameof(SetUnitInfo), RpcTarget.Others, _unit.UnitFlags, Speed);
        ChaseTarget();
    }

    void SetNavStopState(Multi_Enemy newTarget)
    {
        if (gameObject.activeSelf == false) return;
        if (newTarget == null)
            nav.isStopped = true;
        else
            nav.isStopped = false;
    }

    // MasterOnly
    //void SetInfo(UnitFlags flag, UnitStat stat, UnitDamageInfo damInfo)
    //{
    //    _stat = stat;
    //    _unit = new Unit(flag, damInfo, new ObjectSpot(UsingID, true));
    //    UnitAttacker = new UnitAttacker(_unit);
    //    SetUnitInfo(flag, Speed);
    //    photonView.RPC(nameof(SetUnitInfo), RpcTarget.Others, flag, Speed);
    //}

    [PunRPC]
    protected void SetUnitInfo(UnitFlags flag, float speed)
    {
        var stat = new UnitStats(new UnitDamageInfo(), 0, 0, 0, speed);
        _unit = new Unit(flag, stat, new ObjectSpot(UsingID, true)); // 클라에서 speed랑 flag만 채우는 용도
    }

    void OnEnable()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0);
        }

        ChaseTarget();
    }

    void ChaseTarget()
    {
        nav.enabled = true;
        StopAllCoroutines();
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(nameof(NavCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetAiStateValue();
    }

    public void Dead() => photonView.RPC(nameof(RPC_Dead), RpcTarget.All);

    [PunRPC]
    protected void RPC_Dead()
    {
        OnDead?.Invoke(this);
        OnDead = null;
        if(PhotonNetwork.IsMasterClient)
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
        _targetManager.ChangedTarget(TargetFinder.FindTarget(IsInDefenseWorld, transform.position));
        SetNavStopState(TargetEnemy);
        _chaseSystem.ChangedTarget(TargetEnemy);
    }

    public bool contactEnemy = false;
    IEnumerator NavCoroutine()
    {
        if (PhotonNetwork.IsMasterClient == false) yield break;
        
        while (true)
        {
            yield return null;
            if (VaildTargetCondition() == false)
            {
                UpdateTarget();
                continue;
            }

            _chaseSystem.MoveUpdate();
            if ((contactEnemy || MonsterIsForward()) && _state.UnitAttackState.IsAttackable)
                UnitAttack();
        }
    }

    bool VaildTargetCondition() => target != null && _targetManager.Target.IsDead == false && _chaseSystem._currentTarget != null && TargetEnemy.UsingId == UsingID;

    public bool MonsterIsForward() => Physics.RaycastAll(transform.position + Vector3.up, transform.forward, AttackRange).Select(x => x.transform).Contains(target);

    bool isRPC; // RPC딜레이 때문에 공격 2번 이상하는 버그 방지 변수

    void UnitAttack()
    {
        if (isRPC) return;
        isRPC = true;
        photonView.RPC(nameof(Attack), RpcTarget.All);
        isRPC = false;
    }

    [PunRPC]
    protected virtual void Attack() { }

    protected void StartAttack()
    {
        _state.StartAttack();
        AfterPlaySound(normalAttackSound, normalAttakc_AudioDelay);
    }

    bool CheckTargetUpdateCondition => target == null || (TargetIsNormal && (enemyDistance > stopDistanc * 2 || target.GetComponent<Multi_Enemy>().IsDead));
    bool TargetIsNormal => target != null && TargetEnemy.enemyType == EnemyType.Normal;
    protected void EndAttack() => EndSkillAttack(AttackDelayTime);
    protected void EndSkillAttack(float coolTime)
    {
        _state.EndAttack(coolTime);
        if (CheckTargetUpdateCondition) UpdateTarget();
    }

    [SerializeField] protected float enemyDistance => _chaseSystem.EnemyDistance;
    readonly float CHASE_RANGE = 150f;
    protected bool Chaseable => CHASE_RANGE > enemyDistance; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기
    public void ChangeUnitWorld() => photonView.RPC(nameof(ChangeWorld), RpcTarget.All);

    [PunRPC] // PunRPC라 protected 강제임
    protected void ChangeWorld()
    {
        PlaySound(EffectSoundType.UnitTp);
        Managers.Effect.PlayOneShotEffect("UnitTpEffect", gameObject.transform.position + (Vector3.up * 3));
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 destination = GetTpPos();
            base.photonView.RPC(nameof(MoveToOtherWorld), RpcTarget.All, destination);
            _state.ReadyAttack();
            UpdateTarget();
        }
        SettingNav();

        void SettingNav()
        {
            if (IsInDefenseWorld) nav.obstacleAvoidanceType = originObstacleType;
            else nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // 적군의 성이면 충돌 대충 검사
        }
    }
    Vector3 GetTpPos() => IsInDefenseWorld ?
        SpawnPositionCalculator.CalculateTowerWolrdSpawnPostion(Multi_Data.instance.EnemyTowerWorldPositions[Spot.WorldId])
        : SpawnPositionCalculator.CalculateWorldSpawnPostion(Multi_Data.instance.GetWorldPosition(Spot.WorldId));

    [PunRPC]
    protected void MoveToOtherWorld(Vector3 destination)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = destination;
        gameObject.SetActive(true);
        ChangeWolrd(); // 얘는 서순상 여기서 실행되야 해서 RPC안에 넣음
    }

    public void ChangeWorldStateToAll() => photonView.RPC(nameof(ChangeWorldState), RpcTarget.All);
    [PunRPC] protected void ChangeWorldState() => ChangeWolrd();

    void ChangeWolrd() => Spot = Spot.ChangeWorldType();

    protected void AfterPlaySound(EffectSoundType type, float delayTime) => StartCoroutine(Co_AfterPlaySound(type, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType type, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlaySound(type);
    }

    WorldAudioPlayer worldAudioPlayer;
    protected void PlaySound(EffectSoundType type, float volumn = -1) => worldAudioPlayer.PlayObjectEffectSound(Spot, type, volumn);

    [Serializable]
    public class UnitState : MonoBehaviour
    {
        UnitAttackState _unitAttackState = new UnitAttackState();
        public UnitAttackState UnitAttackState => _unitAttackState;

        void Awake()
        {
            ReadyAttack();
        }

        public void Dead() => ReadyAttack();
        public void ReadyAttack() => _unitAttackState = _unitAttackState.ReadyAttack();
        public void StartAttack() => _unitAttackState = _unitAttackState.DoAttack();
 
        public void EndAttack(float coolTime)
        {
            _unitAttackState = _unitAttackState.AttackDone();
            StartCoroutine(Co_AttackCoolDown(coolTime));
        }

        IEnumerator Co_AttackCoolDown(float coolTime)
        {
            yield return new WaitForSeconds(coolTime);
            ReadyAttack();
        }
    }
}

[Serializable]
public class TargetManager
{
    [SerializeField] Multi_Enemy _target;
    public Multi_Enemy Target => _target;

    public void Reset() => ChangedTarget(null);

    public void ChangedTarget(Multi_Enemy newTarget)
    {
        if (_target == newTarget) return;

        if (_target != null)
            _target.OnDead -= ChangedTarget;
        _target = newTarget;
        if (newTarget != null)
        {
            _target.OnDead -= ChangedTarget;
            _target.OnDead += ChangedTarget;
        }
    }
}
