using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    bool rayHit;
    RaycastHit rayHitObject;
    bool enemyIsForward;

    public override bool CanAttack()
    {
        if (enemyIsForward) return true;
        else return false;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up,
            transform.parent.forward , out rayHitObject, attackRange, layerMask);
        if (rayHit)
        {
            if (rayHitObject.transform.gameObject == target.parent.gameObject) enemyIsForward = true;
            else enemyIsForward = false;
        }
        else enemyIsForward = false;
        Stop_or_Move();
    }

    void Stop_or_Move()
    {
        if (enemyIsForward && enemyDistance < stopDistanc)
        {
            nav.isStopped = true;
        }
        else nav.isStopped = false;
        //else if ((Check_EnemyToUnit_Deggre() < -0.6f && enemyDistance < attackRange * 2.2))
        //{
        //    nav.speed = 0.1f;
        //}
    }

    protected float Check_EnemyToUnit_Deggre()
    {
        Enemy enemy = GetEnemyScript();
        float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
        return enemyDot;
    }

    protected void HitMeeleAttack() // 근접공격 타겟팅
    {
        Enemy enemy = GetEnemyScript();
        if (enemy != null && Vector3.Distance(enemy.transform.position, this.transform.position) < attackRange)
            enemy.OnDamage(this.damage);
    }
}
