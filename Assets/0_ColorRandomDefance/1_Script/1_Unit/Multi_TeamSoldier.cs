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
    public UnitFlags UnitFlags => _unit.UnitFlags;

    public UnitClass UnitClass => UnitFlags.UnitClass;
    public UnitColor UnitColor => UnitFlags.UnitColor;

    [SerializeField] UnitStat _stat;

    protected int Damage => _unit.DamageInfo.ApplyDamage;
    protected int BossDamage => _unit.DamageInfo.ApplyBossDamage;
    public float Speed { get => _stat.Speed; set => _stat.SetSpeed(value); }
    public float AttackDelayTime { get => _stat.AttackDelayTime; set => _stat.SetAttDelayTime(value); }
    public float AttackRange => _stat.AttackRange;

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
    protected RPCable rpcable;
    public byte UsingID => (byte)rpcable.UsingId;
    [SerializeField] protected EffectSoundType normalAttackSound;
    public float normalAttakc_AudioDelay;

    protected Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;

    public event Action<Multi_TeamSoldier> OnDead;

    // 가상 함수
    protected virtual void OnAwake() { } // 유닛마다 다른 Awake 세팅
    public virtual void NormalAttack() { } // 유닛들의 고유한 공격
    public virtual void SpecialAttack() => _state.StartAttack();


    [SerializeField] protected TargetManager _targetManager;
    protected UnitState _state;
    public bool EnterStroyWorld => _worldChangeController.EnterStoryWorld;
    public bool IsAttack => _state.UnitAttackState.IsAttack;
    protected ChaseSystem _chaseSystem;

    void Awake()
    {
        // 평타 설정
        OnHit += AttackEnemy;

        // 변수 선언
        passive = GetComponent<Multi_UnitPassive>();
        rpcable = GetComponent<RPCable>();
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        originObstacleType = nav.obstacleAvoidanceType;

        _state = gameObject.AddComponent<UnitState>();
        _chaseSystem = AddCahseSystem();
    }

    void Start()
    {
        OnAwake(); // 유닛별 세팅
    }

    protected virtual ChaseSystem AddCahseSystem() => gameObject.AddComponent<ChaseSystem>();

    public void Spawn(UnitFlags flag, UnitStat stat, UnitDamageInfo damInfo, MonsterManager monsterManager)
    {
        SetInfoToAll();
        _targetManager = new TargetManager(_worldChangeController, transform, monsterManager);
        _targetManager.OnChangedTarget -= SetNewTarget;
        _targetManager.OnChangedTarget += SetNewTarget;
        _targetManager.OnChangedTarget -= _chaseSystem.ChangedTarget;
        _targetManager.OnChangedTarget += _chaseSystem.ChangedTarget;

        SetInfo(flag, stat, damInfo);
        ChaseTarget();
        photonView.RPC(nameof(SetInfoToAll), RpcTarget.Others);
    }

    [PunRPC]
    protected void SetInfoToAll()
    {
        _worldChangeController
            = new WorldChangeController(Multi_Data.instance.GetWorldPosition(rpcable.UsingId), Multi_Data.instance.EnemyTowerWorldPositions[rpcable.UsingId]);
    }

    void SetNewTarget(Multi_Enemy newTarget)
    {
        if (gameObject.activeSelf == false) return;
        if (newTarget == null)
            nav.isStopped = true;
        else
            nav.isStopped = false;
    }

    // MasterOnly
    void SetInfo(UnitFlags flag, UnitStat stat, UnitDamageInfo damInfo)
    {
        _stat = stat;
        _unit = new Unit(flag, damInfo);

        SetUnitInfo(flag, Speed);
        photonView.RPC(nameof(SetUnitInfo), RpcTarget.Others, flag, Speed);
    }

    [PunRPC]
    protected void SetUnitInfo(UnitFlags flag, float speed)
    {
        _unit = new Unit(flag, _unit == null ? new UnitDamageInfo() : _unit.DamageInfo); // 클라 입장에서 flag만 채우는 용도
        SetPassive();
        Speed = speed;
    }


    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _unit.UpdateDamageInfo(newInfo);

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
        // UpdateTarget();
        StopAllCoroutines();
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(nameof(NavCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetAiStateValue();
    }
    
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
        _targetManager.UpdateTarget();
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

    // 최적화 할 때 쓸지도?
    //int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
    //{
    //    int layer = targetObject.layer;
    //    string layerName = LayerMask.LayerToName(layer);
    //    return 1 << LayerMask.NameToLayer(layerName);
    //}

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

    [SerializeField] protected float enemyDistance => _chaseSystem.EnemyDistance;
    readonly float CHASE_RANGE = 150f;
    protected bool Chaseable => CHASE_RANGE > enemyDistance; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기

    protected bool TargetIsNormalEnemy { get { return (target != null && target.GetComponent<Multi_Enemy>().enemyType == EnemyType.Normal); } }

    public void ChangeWorldToMaster() => photonView.RPC(nameof(ChangeWorld), RpcTarget.MasterClient);

    WorldChangeController _worldChangeController;
    // protected 강제임
    [PunRPC]
    protected void ChangeWorld()
    {
        Vector3 destination = _worldChangeController.ChangeWorld(gameObject);
        base.photonView.RPC(nameof(MoveToPos), RpcTarget.Others, destination);
        RPC_PlayTpSound();
        _state.ReadyAttack();
        SettingNav(_worldChangeController.EnterStoryWorld);
        
        UpdateTarget();
        void SettingNav(bool enterStroyWorld)
        {
            if(enterStroyWorld) nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            else nav.obstacleAvoidanceType = originObstacleType;
        }
    }

    [PunRPC]
    protected void MoveToPos(Vector3 pos) => _worldChangeController.ChangeWorld(gameObject, pos);

    public void ChangeWorldStateToAll() => photonView.RPC(nameof(ChangeWorldState), RpcTarget.All);
    [PunRPC] protected void ChangeWorldState() => _worldChangeController.EnterStoryWorld = !_worldChangeController.EnterStoryWorld;

    void RPC_PlayTpSound() // 보는 쪽에서만 소리가 들려야 하므로 복잡해보이는 이 로직이 맞음. 카메라 로직으로 빼서 클라에서 돌리기
    {
        if (rpcable.UsingId == PlayerIdManager.Id)
            Managers.Sound.PlayEffect(EffectSoundType.UnitTp);
        else
            base.photonView.RPC(nameof(PlayTpSound), RpcTarget.Others);
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


    [PunRPC] protected void PlayTpSound() => Managers.Sound.PlayEffect(EffectSoundType.UnitTp);

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


    [Serializable]
    protected class TargetManager
    {
        [SerializeField] Multi_Enemy _target;
        public Multi_Enemy Target => _target;
        public event Action<Multi_Enemy> OnChangedTarget;

        readonly WorldChangeController _worldChangeController;
        Transform _transform;
        MonsterManager _monsterManager;
        int _owerId = -1;
        public TargetManager(WorldChangeController worldChangeController, Transform transform, MonsterManager monsterManager)
        {
            _worldChangeController = worldChangeController;
            _transform = transform;
            _monsterManager = monsterManager;
            _owerId = transform.GetComponent<RPCable>().UsingId;
        }

        public void Reset() => ChangedTarget(null);

        public void UpdateTarget()
        {
            var newTarget = FindTarget();
            if (_target != newTarget)
                ChangedTarget(newTarget);
        }

        Multi_Enemy FindTarget()
        {
            if (_worldChangeController.EnterStoryWorld) 
                return Multi_EnemyManager.Instance.GetCurrnetTower(_owerId);
            if (Multi_EnemyManager.Instance.TryGetCurrentBoss(_owerId, out Multi_BossEnemy boss)) 
                return boss;

            return GetProximateNormalMonster();
        }
        Multi_NormalEnemy GetProximateNormalMonster() => GetProximateEnemys(1).FirstOrDefault();
        public Multi_NormalEnemy[] GetProximateEnemys(int maxCount)
            => _monsterManager.GetNormalMonsters().OrderBy(x => Vector3.Distance(_transform.position, x.transform.position)).Take(maxCount).ToArray();

        void ChangedTarget(Multi_Enemy newTarget)
        {
            if (_target != null)
                _target.OnDead -= ChangedTarget;
            _target = newTarget;
            OnChangedTarget?.Invoke(newTarget);
            if(newTarget != null)
            {
                _target.OnDead -= ChangedTarget;
                _target.OnDead += ChangedTarget;
            }
        }
    }
}
