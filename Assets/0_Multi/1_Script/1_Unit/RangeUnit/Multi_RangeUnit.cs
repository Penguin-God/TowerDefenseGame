using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_RangeUnit : Multi_TeamSoldier
{
    protected override Vector3 DestinationPos
    {
        get
        {
            Vector3 enemySpeed = TargetEnemy.dir * TargetEnemy.speed;
            return target.position + enemySpeed;
        }
    }

    protected override bool IsMoveLock => AttackRange * 0.8f >= enemyDistance;

    public override void UnitTypeMove()
    {
        if (IsMoveLock) LockMove();
        else
        {
            if (nav.updatePosition == false)
                ReleaseMove();
            nav.speed = Speed;
        }
    }

    private void FixedUpdate()
    {
        // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        rayHit = Physics.BoxCast(transform.position + Vector3.up, transform.lossyScale * 2,
            transform.forward, out rayHitObject, transform.rotation, AttackRange, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * AttackRange);
        Gizmos.DrawWireCube(transform.position + Vector3.up + transform.forward * AttackRange, transform.lossyScale * 2);
    }
}
