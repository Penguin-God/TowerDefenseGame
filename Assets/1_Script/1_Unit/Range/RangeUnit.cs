using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeUnit : TeamSoldier
{
    public bool isFix = false;
    public override void UnitTypeMove()
    {
        if (enterStoryWorld) return;

        if (enemyDistance < attackRange)
        {
            if(!target.gameObject.CompareTag("Tower")) nav.speed = 0.1f;

            if(enemyDistance < stopDistanc) contactEnemy = true;
            else contactEnemy = false;
        }
        else nav.speed = this.speed;
    }

    protected GameObject CreateBullte(GameObject instantObject, Transform createPositon, Delegate_OnHit OnDamage)
    {
        Vector3 instantPosition = new Vector3(createPositon.position.x, 2f, createPositon.position.z);
        GameObject instantBullet = Instantiate(instantObject, instantPosition, (unitType == Type.archer) ? Quaternion.identity : transform.parent.rotation);

        CollisionWeapon weapon = instantBullet.GetComponent<CollisionWeapon>();
        if (weapon != null) weapon.UnitOnDamage += (Enemy enemy) => OnDamage(enemy);
        else Debug.LogWarning("아니 CollisionWeapon가 읎어요");
        return instantBullet;
    }

    // 원거리 유닛 무기 발사
    protected void ShotBullet(GameObject bullet, float weightRate, float velocity, Transform targetEnemy)
    {
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        Vector3 dir;
        // 속도 가중치 설정(적보다 약간 앞을 쏨)
        if (targetEnemy != null)
        {
            dir = targetEnemy.position - bullet.transform.position;
            float enemyWeightDir = Mathf.Lerp(0, weightRate, Vector3.Distance(targetEnemy.position, this.transform.position) * 2 / 100);
            dir += nomalEnemy.dir.normalized * (0.5f * nomalEnemy.speed) * enemyWeightDir;
        }
        else dir = bullet.transform.forward;

        bulletRigid.velocity = dir.normalized * velocity;
    }

    private void FixedUpdate()
    {
        // Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        rayHit = Physics.BoxCast(transform.parent.position + Vector3.up, transform.lossyScale * 2,
            transform.parent.forward, out rayHitObject, transform.parent.rotation, attackRange, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange);
        Gizmos.DrawWireCube(transform.parent.position + Vector3.up + transform.parent.forward * attackRange, transform.lossyScale * 2);
    }
}
