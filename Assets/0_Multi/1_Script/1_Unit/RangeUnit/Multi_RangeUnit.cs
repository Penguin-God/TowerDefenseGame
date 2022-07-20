using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_RangeUnit : Multi_TeamSoldier
{
    public override Vector3 DestinationPos
    {
        get
        {
            Vector3 enemySpeed = TargetEnemy.dir * TargetEnemy.speed;
            return target.position + enemySpeed;
        }
    }

    public override void UnitTypeMove()
    {
        if (enterStoryWorld) return;

        if (enemyDistance < AttackRange)
        {
            if (!target.gameObject.CompareTag("Tower")) nav.speed = 0.1f;

            if (enemyDistance < stopDistanc) contactEnemy = true;
            else contactEnemy = false;
        }
        else nav.speed = Speed;
    }

    // 원거리 유닛 무기 발사
    protected Vector3 Get_ShootDirection(float weightRate, Transform _target)
    {
        Vector3 dir;
        // 속도 가중치 설정(적보다 약간 앞을 쏨, 적군의 성 공격할 때는 의미 없음)
        if (_target != null)
        {
            dir = _target.position - transform.position;
            float enemyWeightDir = Mathf.Lerp(0, weightRate, Vector3.Distance(_target.position, this.transform.position) * 2 / 100);
            dir += TargetEnemy.dir.normalized * (0.5f * TargetEnemy.speed) * enemyWeightDir;
        }
        else dir = transform.forward;

        return dir.normalized;
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
