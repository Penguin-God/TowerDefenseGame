using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up, transform.parent.forward, out rayHitObject, attackRange, layerMask);
    }

    public override void UnitTypeMove()
    {
        // 정지조건 3개
        //if (enemyIsForward && target.gameObject.tag == "Tower")
        //{
        //    nav.isStopped = true;
        //}
        //else nav.isStopped = false;

        if (enemyDistance < stopDistanc) nav.speed = 0.15f;
        else nav.speed = this.speed;
    }

    //protected float Check_EnemyToUnit_Deggre()
    //{
    //    if (nomalEnemy == null) return 1f;
    //    float enemyDot = Vector3.Dot(nomalEnemy.dir.normalized, (target.position - this.transform.position).normalized);
    //    return enemyDot;
    //}

    public virtual void MeeleUnit_PassiveAttack(Enemy enemy)
    {

    }

    protected void HitMeeleAttack() // 근접공격 타겟팅, damage는 델리게이트 통일을 위한 잉여 변수
    {
        // 공격 시작 때 적과 HitMeeleAttack() 작동 시 적과 같은 적인지 비교하는 코드 필요
        Enemy enemy = GetEnemyScript();
        if (enemy != null && (enemyDistance < attackRange * 1.5f || target.gameObject.CompareTag("Tower") || target.gameObject.CompareTag("Boss") ) )
        {
            AttackEnemy(enemy);
            MeeleUnit_PassiveAttack(enemy);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (target == null) return;

        if ( other.gameObject.layer == 8 && other.gameObject == target.gameObject && !other.gameObject.CompareTag("Tower") )
        {
            contactEnemy = true;
        }
        else
        {
            contactEnemy = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject == target.gameObject && !other.gameObject.CompareTag("Tower"))
        {
            contactEnemy = false;
        }
    }
}
