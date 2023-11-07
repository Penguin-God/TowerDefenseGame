using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;
using System.Linq;

public enum UnitColor { Red, Blue, Yellow, Green, Orange, Violet, White, Black };
public enum UnitClass { Swordman, Archer, Spearman, Mage }

public class Multi_TeamSoldier : MonoBehaviourPun
{
    [SerializeField] Unit _unit;
    public Unit Unit => _unit;
    public UnitFlags UnitFlags => _unit.UnitFlags;

    public ObjectSpot Spot => _state.Spot;
    public bool IsInDefenseWorld => Spot.IsInDefenseWorld;

    public UnitClass UnitClass => UnitFlags.UnitClass;
    public UnitColor UnitColor => UnitFlags.UnitColor;
    
    public int BossDamage => _unit.DamageInfo.ApplyBossDamage;
    public float Speed => Unit.Stats.Speed;
    public float AttackDelayTime => Unit.Stats.AttackDelayTime;
    public float AttackRange => Unit.Stats.AttackRange;

    [SerializeField] protected float stopDistanc;

    public Transform target => _targetManager.Target == null ? null : TargetEnemy.transform;
    public Multi_Enemy TargetEnemy => _targetManager.Target;

    protected NavMeshAgent nav;
    private ObstacleAvoidanceType originObstacleType;
    protected Animator animator;
    protected RPCable rpcable;
    public byte UsingID => (byte)rpcable.UsingId;
    public event Action<Multi_TeamSoldier> OnDead;

    // 가상 함수
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    
    [SerializeField] protected TargetManager _targetManager = new TargetManager();
    public UnitStateManager _state;
    public bool IsAttack => _state.UnitAttackState.IsAttack;
    protected UnitChaseSystem _chaseSystem;

    void Awake()
    {
        // 변수 선언
        rpcable = GetComponent<RPCable>();
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        originObstacleType = nav.obstacleAvoidanceType;
        worldAudioPlayer = gameObject.AddComponent<WorldAudioPlayer>();
        worldAudioPlayer.DependencyInject(Managers.Camera, Managers.Sound);
    }

    void Start()
    {
        OnAwake(); // 유닛별 세팅
        gameObject.AddComponent<UnitStateSync>();
    }

    protected MonsterFinder TargetFinder { get; private set; }
    public UnitAttacker UnitAttacker { get; private set; }

    public void Injection(Unit unit, MonsterManagerController monsterManager)
    {
        TargetFinder = new MonsterFinder(monsterManager, UsingID);
        _unit = unit;
        _state = new UnitStateManager(new ObjectSpot(UsingID, true));
        UnitAttacker = new UnitAttacker(_unit, UsingID);
        ChaseTarget();
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
        StartCoroutine(nameof(NavCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetValue();
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

    protected virtual void ResetValue()
    {
        _targetManager.Reset();
        contactEnemy = false;

        if (animator != null)
            animator.enabled = false;

        nav.enabled = false;
    }

    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        _targetManager.ChangedTarget(TargetFinder.FindTarget(IsInDefenseWorld, transform.position));
        _chaseSystem.ChangedTarget(TargetEnemy);
        // SetNavStopState();
    }

    void SetNavStopState()
    {
        if (gameObject.activeSelf == false) return;
        if (target == null)
            nav.isStopped = true;
        else
            nav.isStopped = false;
    }

    public bool contactEnemy = false;
    IEnumerator NavCoroutine()
    {
        while (true)
        {
            yield return null;
            if (VaildTargetCondition() == false)
            {
                UpdateTarget();
                continue;
            }

            _chaseSystem.MoveUpdate();
            if (PhotonNetwork.IsMasterClient == false) continue;

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
        AttackToAll();
        isRPC = false;
    }

    protected virtual void AttackToAll() => photonView.RPC(nameof(Attack), RpcTarget.All);
    [PunRPC]
    protected virtual void Attack() { }
    public void _NormalAttack()
    {
        if(PhotonNetwork.IsMasterClient)
            UnitAttacker.NormalAttack(TargetEnemy);
    }

    public void ChangeUnitWorld() => photonView.RPC(nameof(ChangeWorld), RpcTarget.All);

    [PunRPC] // PunRPC라 protected 강제임
    protected void ChangeWorld()
    {
        PlaySound(EffectSoundType.UnitTp);
        Managers.Effect.PlayOneShotEffect("UnitTpEffect", gameObject.transform.position + (Vector3.up * 3));
        if (PhotonNetwork.IsMasterClient)
        {
            base.photonView.RPC(nameof(MoveToOtherWorld), RpcTarget.All, GetTpPos());
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
        SpawnPositionCalculator.CalculateTowerWolrdSpawnPostion(MultiData.instance.EnemyTowerWorldPositions[Spot.WorldId])
        : SpawnPositionCalculator.CalculateWorldSpawnPostion(MultiData.instance.GetWorldPosition(Spot.WorldId));

    [PunRPC]
    protected void MoveToOtherWorld(Vector3 destination)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = destination;
        gameObject.SetActive(true);
        _state.ChangeWorld(); // 얘는 서순상 여기서 실행되야 해서 RPC안에 넣음
    }

    public void ChangeWorldStateToAll() => photonView.RPC(nameof(ChangeWorldState), RpcTarget.All);
    [PunRPC] protected void ChangeWorldState() => _state.ChangeWorld();

    public void AfterPlaySound(EffectSoundType type, float delayTime) => StartCoroutine(Co_AfterPlaySound(type, delayTime));

    IEnumerator Co_AfterPlaySound(EffectSoundType type, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlaySound(type);
    }

    WorldAudioPlayer worldAudioPlayer;
    void PlaySound(EffectSoundType type, float volumn = -1) => worldAudioPlayer.PlayObjectEffectSound(Spot, type, volumn);
}

[Serializable]
public class UnitStateManager
{
    UnitAttackState _unitAttackState = new UnitAttackState(true, false);
    public UnitAttackState UnitAttackState => _unitAttackState;

    public ObjectSpot Spot { get; private set; }
    public UnitStateManager(ObjectSpot spot) => Spot = spot;

    public void Dead() => ReadyAttack();
    public void StartAttack() => _unitAttackState = _unitAttackState.DoAttack();
    public void ReadyAttack() => _unitAttackState = _unitAttackState.ReadyAttack();
    public void EndAttack() => _unitAttackState = _unitAttackState.AttackDone();
    public void ChangeWorld()
    {
        Spot = Spot.ChangeWorldType();
        ReadyAttack();
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
