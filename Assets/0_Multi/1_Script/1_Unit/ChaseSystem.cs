using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum ChaseState
{
    NoneTarget,
    Chase,
    InRange,
    Lock,
    FaceToFace,
}

public class ChaseSystem : MonoBehaviourPun, IPunObservable
{
    protected Multi_TeamSoldier _unit { get; private set; }
    protected NavMeshAgent _nav { get; private set; }

    protected Multi_Enemy _currentTarget = null;
    protected Vector3 TargetPosition => _currentTarget.transform.position;
    public virtual void ChangedTarget(Multi_Enemy newTarget)
    {
        if (newTarget == null)
        {
            _currentTarget = null;
            enemyDistance = Mathf.Infinity;
            _chaseState = ChaseState.NoneTarget;
            return;
        }

        _currentTarget = newTarget;
        layerMask = ReturnLayerMask(_currentTarget.gameObject);
    }

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _unit = GetComponent<Multi_TeamSoldier>();
        photonView.ObservedComponents.Add(this);
    }

    [SerializeField] protected ChaseState _chaseState;
    [SerializeField] protected float enemyDistance;
    public float EnemyDistance => enemyDistance;
    protected virtual Vector3 GetDestinationPos() => Vector3.zero;
    protected virtual ChaseState GetChaseState() => ChaseState.NoneTarget;
    protected virtual void SetChaseStatus(ChaseState state) { }

    [SerializeField] Vector3 chasePosition;
    public void MoveUpdate()
    {
        if (_currentTarget == null) return;

        UpdateState();
        chasePosition = GetDestinationPos();
        enemyDistance = Vector3.Distance(transform.position, chasePosition);
        _nav.SetDestination(chasePosition);
    }

    void UpdateState()
    {
        var newState = GetChaseState();
        if (_chaseState != newState)
        {
            _chaseState = newState;
            SetChaseStatus(_chaseState);
            //photonView.RPC(nameof(ClineStateUpdate), RpcTarget.Others, (byte)_chaseState);
        }
    }

    [PunRPC]
    public void ClineStateUpdate(byte newState) => SetChaseStatus((ChaseState)newState);

    void FixedUpdate()
    {
        if (_currentTarget == null) return;
        enemyIsForward = ChcekEnemyInSight();
    }

    [SerializeField] protected bool enemyIsForward;
    public bool EnemyIsForward => enemyIsForward;
    protected int layerMask; // Ray 감지용
    readonly float CHASE_RANGE = 150f;
    protected bool Chaseable => CHASE_RANGE > enemyDistance && _currentTarget != null; // 거리가 아닌 다른 조건(IsDead 등)으로 바꾸기

    protected virtual bool RaycastEnemy(out Transform hitEnemy)
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit rayHitObject, 5, layerMask) == false)
        {
            hitEnemy = null;
            return false;
        }

        hitEnemy = rayHitObject.transform;
        return true;
    }
    bool ChcekEnemyInSight()
    {
        if (RaycastEnemy(out Transform hitEnemy) == false) return false;

        if (TransformIsBoss(hitEnemy) || hitEnemy == _currentTarget)
            return true;
        // ray에 맞은 적이 target은 아니지만 target과 같은 layer라면 두 enemy가 겹친 것으로 판단해 true를 리턴
        else if (ReturnLayerMask(hitEnemy.gameObject) == layerMask && Vector3.Distance(TargetPosition, hitEnemy.position) < 5f)
            return true;

        return false;
    }
    bool TransformIsBoss(Transform enemy) => enemy.CompareTag("Tower") || enemy.CompareTag("Boss");

    int ReturnLayerMask(GameObject targetObject) // 인자의 layer를 반환하는 함수
    {
        int layer = targetObject.layer;
        string layerName = LayerMask.LayerToName(layer);
        return 1 << LayerMask.NameToLayer(layerName);
    }

    Vector3 _prevSendChasePosition;
    ChaseState _prevSendState;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(chasePosition);
            _prevSendChasePosition = chasePosition;
            stream.SendNext((byte)_chaseState);
            _prevSendState = _chaseState;
        }
        else
        {
            _nav.SetDestination((Vector3)stream.ReceiveNext());
            _chaseState = (ChaseState)(byte)stream.ReceiveNext();
            SetChaseStatus(_chaseState);
            _nav.isStopped = _chaseState == ChaseState.NoneTarget;
        }
    }
}


public class MeeleChaser : ChaseSystem
{
    Vector3 currentDestinationPos;
    protected override Vector3 GetDestinationPos()
    {
        if (_unit.EnterStroyWorld) return currentDestinationPos;

        switch (_chaseState)
        {
            case ChaseState.Chase: return currentDestinationPos = TargetPosition - (_currentTarget.dir * 1);
            case ChaseState.InRange: return currentDestinationPos = TargetPosition - (_currentTarget.dir * 2);
            case ChaseState.FaceToFace: return currentDestinationPos = TargetPosition - (_currentTarget.dir * -5f);
            case ChaseState.Lock: return currentDestinationPos = TargetPosition - (_currentTarget.dir * -1f);
        }

        return currentDestinationPos;
    }

    protected override void SetChaseStatus(ChaseState state)
    {
        switch (state)
        {
            case ChaseState.Chase:
                _nav.speed = _unit.Speed;
                _nav.angularSpeed = 500;
                _nav.acceleration = 40;
                _unit.contactEnemy = false;
                break;
            case ChaseState.InRange:
                _nav.acceleration = 20f;
                _nav.angularSpeed = 200;
                _nav.speed = 5f;
                _unit.contactEnemy = true;
                break;
            case ChaseState.FaceToFace:
                _nav.acceleration = 20f;
                _nav.angularSpeed = 500;
                _nav.speed = 15f;
                break;
            case ChaseState.Lock:
                _nav.acceleration = 2f;
                _nav.angularSpeed = 5;
                _nav.speed = 1f;
                break;
        }
    }

    float Check_EnemyToUnit_Deggre()
    {
        if (_currentTarget == null) return 1f;
        float enemyDot = Vector3.Dot(_currentTarget.dir.normalized, (currentDestinationPos - transform.position));
        return enemyDot;
    }

    protected override ChaseState GetChaseState()
    {
        if (Check_EnemyToUnit_Deggre() < -0.8f && enemyDistance < 10)
        {
            if (enemyIsForward || _unit.IsAttack) return ChaseState.Lock;
            else return ChaseState.FaceToFace;
        }
        else if (5 > enemyDistance) return ChaseState.InRange;
        else return ChaseState.Chase;
    }

    protected override bool RaycastEnemy(out Transform hitEnemy)
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit rayHitObject, 5, layerMask) == false)
        {
            hitEnemy = null;
            return false;
        }

        hitEnemy = rayHitObject.transform;
        return true;
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * _unit.AttackRange, Color.green);
    }

    public override void ChangedTarget(Multi_Enemy newTarget)
    {
        if (newTarget == null) return;
        base.ChangedTarget(newTarget);
        if (newTarget.enemyType == EnemyType.Tower) ChaseTower(newTarget);
    }

    void ChaseTower(Multi_Enemy tower)
    {
        if (tower != null)
        {
            if (Physics.Raycast(transform.position, TargetPosition - transform.position, out RaycastHit towerHit, 50f, layerMask))
                currentDestinationPos = towerHit.point;
            else
                currentDestinationPos = transform.position;
        }
    }
}


public class RangeChaser : ChaseSystem
{
    protected override Vector3 GetDestinationPos()
    {
        Vector3 enemySpeed = _currentTarget.dir * 2;
        return TargetPosition + enemySpeed;
    }

    protected override void SetChaseStatus(ChaseState state)
    {
        switch (state)
        {
            case ChaseState.Chase:
                if (_nav.updatePosition == false)
                    ReleaseMove();
                _nav.speed = _unit.Speed;
                break;
            case ChaseState.Lock:
                LockMove();
                break;
        }
    }

    void LockMove()
    {
        if (_nav.updatePosition == false) return;
        _nav.updatePosition = false;
    }

    void ReleaseMove()
    {
        if (_nav.updatePosition == true) return;

        ResetNavPosition();
        _nav.updatePosition = true;
    }

    void ResetNavPosition()
    {
        _nav.Warp(transform.position);
        if (_currentTarget != null)
            _nav.SetDestination(TargetPosition);
    }

    void Update() => FixedNavPosition();
    readonly float MAX_NAV_OFFSET = 3f;
    void FixedNavPosition()
    {
        if (Vector3.Distance(_nav.nextPosition, transform.position) > MAX_NAV_OFFSET)
            ResetNavPosition();
    }

    protected virtual bool IsMoveLock => _unit.AttackRange * 0.8f >= enemyDistance || _unit.IsAttack;
    protected override ChaseState GetChaseState()
    {
        if (IsMoveLock) return ChaseState.Lock;
        else return ChaseState.Chase;
    }

    protected override bool RaycastEnemy(out Transform hitEnemy)
    {
        // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        if (Physics.BoxCast(transform.position + Vector3.up, transform.lossyScale * 2, transform.forward, 
            out RaycastHit rayHitObject, transform.rotation, _unit.AttackRange, layerMask) == false)
        {
            hitEnemy = null;
            return false;
        }

        hitEnemy = rayHitObject.transform;
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * _unit.AttackRange);
        Gizmos.DrawWireCube(transform.position + Vector3.up + transform.forward * _unit.AttackRange, transform.lossyScale * 2);
    }
}
