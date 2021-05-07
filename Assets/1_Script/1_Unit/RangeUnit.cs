using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit : TeamSoldier
{ 
    public override bool CanAttack()
    {
        if (enemyIsForward) return true;
        else return false;
    }

    private void Update()
    {
        RangeChaseMove(enemyDistance);
    }

    //bool rayHit;
    //RaycastHit rayHitObject;
    //bool enemyIsForward;
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up, transform.parent.forward, out rayHitObject, attackRange, layerMask);
        if (rayHit)
        {
            //Debug.Log(rayHitObject.transform.gameObject);
            if (rayHitObject.transform.gameObject == target.parent.gameObject) enemyIsForward = true;
            else enemyIsForward = false;
        }
    }

    void RangeChaseMove(float distance)
    {
        if (distance < attackRange) nav.speed = 0.1f;
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

    protected void ShotBullet(GameObject bullet, float weightRate, float velocity) // 원거리 유닛 총알 발사
    {
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        Enemy enemy = GetEnemyScript();

        Vector3 dir = target.position - bullet.transform.position;
        float enemyWeightDir = Mathf.Lerp(0, enemy.speed, (weightRate * Vector3.Distance(target.position, this.transform.position)) / 100);
        dir += enemy.dir * enemyWeightDir;
        bulletRigid.velocity = dir.normalized * velocity;
    }
}
