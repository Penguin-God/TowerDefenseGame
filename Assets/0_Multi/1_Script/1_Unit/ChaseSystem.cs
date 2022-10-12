using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseSystem : MonoBehaviour
{
    protected NavMeshAgent _nav;
    [SerializeField] protected Multi_Enemy _currentTarget = null;
    protected Vector3 TargetPosition => _currentTarget.transform.position;
    public void ChangedTarget(Multi_Enemy newTarget)
    {
        if (newTarget == null)
        {
            _currentTarget = null;
            return;
        }

        _currentTarget = newTarget;
        layerMask = ReturnLayerMask(_currentTarget.gameObject);
    }

    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        Init();
    }

    protected virtual void Init() {}

    protected virtual Vector3 GetDestinationPos() => Vector3.zero;
    protected virtual void Move() => Debug.Log("선언이 안되있을지도?");
    public void MoveUpdate()
    {
        if (_currentTarget == null) return;

        enemyDistance = Vector3.Distance(transform.position, GetDestinationPos());
        FixedNavPosition();
        _nav.SetDestination(GetDestinationPos());
        enemyIsForward = ChcekEnemyInSight();
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
        _nav.Warp(transform.position);
        if (_currentTarget != null)
            _nav.SetDestination(TargetPosition);
    }

    readonly float MAX_NAV_OFFSET = 3f;
    void FixedNavPosition()
    {
        if (Vector3.Distance(_nav.nextPosition, transform.position) > MAX_NAV_OFFSET)
            ResetNavPosition();
    }

    [SerializeField] protected float enemyDistance;
    [SerializeField] protected bool enemyIsForward;

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
}


public class MeeleChaser : ChaseSystem
{
    Multi_TeamSoldier _unit;
    protected override void Init()
    {
        _unit = GetComponent<Multi_TeamSoldier>();
    }

    Vector3 currentDestinationPos;
    protected override Vector3 GetDestinationPos()
    {
        if (Check_EnemyToUnit_Deggre() < -0.8f && enemyDistance < 10)
        {
            if (enemyIsForward || _unit.IsAttack)
            {
                _nav.acceleration = 2f;
                _nav.angularSpeed = 5;
                _nav.speed = 1f;
                currentDestinationPos = TargetPosition - (_currentTarget.dir * -1f);
            }
            else
            {
                _nav.acceleration = 20f;
                _nav.angularSpeed = 500;
                _nav.speed = 15f;
                currentDestinationPos = TargetPosition - (_currentTarget.dir * -5f);
            }
        }
        else if (5 > enemyDistance)
        {
            _nav.acceleration = 20f;
            _nav.angularSpeed = 200;
            _nav.speed = 5f;
            _unit.contactEnemy = true;
            currentDestinationPos = TargetPosition - (_currentTarget.dir * 2);
        }
        else
        {
            _nav.speed = _unit.Speed;
            _nav.angularSpeed = 500;
            _nav.acceleration = 40;
            _unit.contactEnemy = false;
            currentDestinationPos = TargetPosition - (_currentTarget.dir * 1);
        }

        return currentDestinationPos;
    }

    float Check_EnemyToUnit_Deggre()
    {
        if (_currentTarget == null) return 1f;
        float enemyDot = Vector3.Dot(_currentTarget.dir.normalized, (currentDestinationPos - transform.position));
        return enemyDot;
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
}
