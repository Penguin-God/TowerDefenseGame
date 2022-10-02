﻿using System.Collections;
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

    public bool isAttack; // 공격 중에 true
    public bool isAttackDelayTime; // 공격 쿨타임 중에 true
    // 나중에 유닛별 공격 조건 만들면서 없애기
    public bool isSkillAttack; // 스킬 공격 중에 true
    public float skillCoolDownTime;

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

    protected float chaseRange; // 풀링할 때 멀리 풀에 있는 놈들 충돌 안하게 하기위한 추적 최소거리

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
        if (PhotonNetwork.IsMasterClient) StartCoroutine("NavCoroutine");
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
        ResetValueSataeValue();
    }

    void ResetValueSataeValue()
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

    // 현재 살아있는 enemy 중 가장 가까운 enemy의 정보를 가지고 nav 및 변수 설정
    public void UpdateTarget() // 가장 가까운 거리에 있는 적으로 타겟을 바꿈
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        if (enterStoryWorld)
        {
            SetTargetByTower();
            return;
        }

        Transform _target = Multi_EnemyManager.Instance.GetProximateEnemy(transform.position, chaseRange, GetComponent<RPCable>().UsingId);
        if (_target != null) SetChaseSetting(_target.gameObject);
        else SetChaseSetting(null);
    }

    public void SetChaseSetting(GameObject targetObject) // 추적 관련 변수 설정
    {
        if (PhotonNetwork.IsMasterClient == false) return;

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
        if (target != null && TargetIsNormalEnemy && enemyDistance > stopDistanc * 2) UpdateTarget();
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
        //nav.speed = 0.1f;
        nav.updatePosition = false;
    }

    protected void ReleaseMove()
    {
        if (nav.updatePosition == true) return;

        ResetNavPosition();
        //nav.speed = Speed;
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
        photonView.RPC("UpdateStatus", RpcTarget.All, enterStoryWorld);
        if (enterStoryWorld) EnterStroyMode();
        else EnterWolrd();

        rpcable.SetActive_RPC(true);
        Multi_Managers.Sound.PlayEffect(EffectSoundType.UnitTp);

        return; // 중복함수 구분용 return : 의미 없음

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
    }

    [PunRPC]
    protected void UpdateStatus(bool isEnterStroyMode)
    {
        enterStoryWorld = isEnterStroyMode;
    }

    void EnterStroyMode()
    {
        nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        SetTargetByTower();
    }

    void SetTargetByTower()
    {
        Multi_EnemyTower tower = Multi_EnemyManager.Instance.GetCurrnetTower(GetComponent<RPCable>().UsingId);
        if (tower != null)
        {
            SetChaseSetting(tower.gameObject);
            if(Physics.Raycast(transform.position, target.position - transform.position, out RaycastHit towerHit, 50f, layerMask))
                DestinationPos = towerHit.point;
            else
                DestinationPos = transform.position;
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
}
