using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseSystem : MonoBehaviour
{
    NavMeshAgent _nav;
    [SerializeField] Multi_Enemy _currentTarget = null;
    Vector3 TargetPosition => _currentTarget.transform.position;
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
    }

    protected virtual Vector3 DestinationPos { get; set; }
    protected virtual void Move() => Debug.Log("선언이 안되있을지도?");
    protected virtual bool ShotRay() => false;
    public void MoveUpdate()
    {
        if (_currentTarget == null) return;

        enemyDistance = Vector3.Distance(transform.position, TargetPosition);
        FixedNavPosition();
        Move();
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
