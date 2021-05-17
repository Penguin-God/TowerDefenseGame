using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit : TeamSoldier
{ 
    public override void UnitTypeMove()
    {
        if (enemyDistance < attackRange) nav.speed = 0.1f;
        else nav.speed = this.speed;
    }

    protected GameObject CreateBullte(GameObject instantObject, Transform createPositon)
    {
        Vector3 instantPosition = new Vector3(createPositon.position.x, 2f, createPositon.position.z);
        GameObject instantBullet = Instantiate(instantObject, instantPosition, (unitType == Type.archer) ? Quaternion.identity : transform.parent.rotation);

        AttackWeapon attackWeapon = instantBullet.GetComponent<AttackWeapon>();
        attackWeapon.attackUnit = this.gameObject; // 화살과 적의 충돌감지를 위한 대입
        return instantBullet;
    }

    protected void ShotBullet(GameObject bullet, float weightRate, float velocity, Transform targetEnemy) // 원거리 유닛 총알 발사
    {
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();

        Vector3 dir = targetEnemy.position - bullet.transform.position;
        float enemyWeightDir = Mathf.Lerp(0, weightRate, Vector3.Distance(targetEnemy.position, this.transform.position) * 2 / 100 );
        dir += nomalEnemy.dir.normalized * (0.5f * nomalEnemy.speed) * enemyWeightDir;
        //Debug.Log(enemyWeightDir);
        bulletRigid.velocity = dir.normalized * velocity;
    }

    public virtual void RangeUnit_PassiveAttack(Enemy enemy)
    {

    }

    private void FixedUpdate()
    {
        rayHit = Physics.BoxCast(transform.parent.position + Vector3.up, transform.lossyScale,
            transform.parent.forward, out rayHitObject, transform.parent.rotation, attackRange, layerMask);
    }

    void OnDrawGizmos()
    {
        //RaycastHit hit;
        //// Physics.BoxCast (레이저를 발사할 위치, 사각형의 각 좌표의 절판 크기, 발사 방향, 충돌 결과, 회전 각도, 최대 거리)
        //bool isHit = Physics.BoxCast(transform.parent.position, transform.lossyScale * 10,
        //    transform.parent.forward, out hit, transform.parent.rotation, attackRange);
        //if (isHit) Debug.Log("맞았다!!!!!!!!!!!");

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange);
        Gizmos.DrawWireCube(transform.parent.position + Vector3.up + transform.parent.forward * attackRange, transform.lossyScale);
    }
}
