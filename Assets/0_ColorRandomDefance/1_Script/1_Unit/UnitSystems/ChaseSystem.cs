using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ChaseSystem : MonoBehaviourPun, IPunObservable
{
    protected Multi_TeamSoldier _unit { get; private set; }
    protected NavMeshAgent _nav { get; private set; }

    public Multi_Enemy _currentTarget = null;
    protected Vector3 TargetPosition => _currentTarget.transform.position;
    protected UnitChaseUseCase _unitChaseUseCase;

    public virtual void ChangedTarget(Multi_Enemy newTarget)
    {
        if (newTarget == null)
        {
            _currentTarget = null;
            _chaseState = ChaseState.NoneTarget;
            return;
        }

        _currentTarget = newTarget;
    }

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _unit = GetComponent<Multi_TeamSoldier>();
        _unitChaseUseCase = new UnitChaseUseCase(_unit.AttackRange);
        photonView.ObservedComponents.Add(this);
    }

    [SerializeField] protected ChaseState _chaseState;
    public float EnemyDistance => Vector3.Distance(transform.position, chasePosition);
    protected virtual Vector3 GetDestinationPos() => Vector3.zero;
    protected virtual ChaseState GetChaseState() => ChaseState.NoneTarget;
    protected virtual void SetChaseStatus(ChaseState state) { }

    [SerializeField] protected Vector3 chasePosition;
    public void MoveUpdate()
    {
        if (_currentTarget == null) return;

        UpdateState();
        chasePosition = (_currentTarget.enemyType == EnemyType.Tower) ? ChaseTower() : GetDestinationPos();
        _nav.SetDestination(chasePosition);
    }

    Vector3 ChaseTower()
    {
        if (Physics.Raycast(transform.position, TargetPosition - transform.position, out RaycastHit towerHit, 50f))
            return Vector3.Lerp(transform.position, towerHit.point, 0.9f);
        else
            return transform.position;
    }

    void UpdateState()
    {
        var newState = GetChaseState();
        if (_chaseState != newState)
        {
            _chaseState = newState;
            SetChaseStatus(_chaseState);
        }
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
    protected override Vector3 GetDestinationPos()
        => new UnitChaseUseCase(_unit.AttackRange).CalculateDestinationPos(_chaseState, TargetPosition, _currentTarget.dir);

    protected override void SetChaseStatus(ChaseState state)
    {
        switch (state)
        {
            case ChaseState.Far: ChangeNavState(_unit.Speed, 500, 40, false); break;
            case ChaseState.Close: ChangeNavState(10, 500, 20, false); break;
            case ChaseState.Contact:
            case ChaseState.InRange: ChangeNavState(5, 200, 20, true); break;
            case ChaseState.FaceToFace: ChangeNavState(15, 500, 20, false); break;
            case ChaseState.Lock: ChangeNavState(1, 2, 5, true); break;
        }

        void ChangeNavState(float speed, float angularSpeed, float acceleration, bool isContact)
        {
            _nav.speed = speed;
            _nav.angularSpeed = angularSpeed;
            _nav.acceleration = acceleration;
            _unit.contactEnemy = isContact;
        }
    }

    protected override ChaseState GetChaseState()
    {
        var state = new UnitChaseUseCase(_unit.AttackRange).CalculateChaseState(transform.position, chasePosition, transform.forward, _currentTarget.dir);
        if (state == ChaseState.FaceToFace && (_unit.MonsterIsForward() || _unit.IsAttack)) return ChaseState.Lock;
        else return state;
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

    readonly float MAX_NAV_OFFSET = 3f;
    bool NavIsOutRange() => Vector3.Distance(_nav.nextPosition, transform.position) > MAX_NAV_OFFSET;
    void Update()
    {
        if (NavIsOutRange())
            ResetNavPosition();
    }

    protected virtual bool IsMoveLock => _unit.AttackRange * 0.8f >= EnemyDistance || _unit.IsAttack;
    protected override ChaseState GetChaseState()
    {
        if (IsMoveLock) return ChaseState.Lock;
        else return ChaseState.Chase;
    }
}
