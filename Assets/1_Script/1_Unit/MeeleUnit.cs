using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    //public override bool CanAttack()
    //{
    //    if (enemyIsForward) return true;
    //    else return false;
    //}

    //private void FixedUpdate()
    //{
    //    Debug.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
    //    rayHit = Physics.Raycast(transform.parent.position + Vector3.up, transform.parent.forward , out rayHitObject, attackRange, layerMask);
    //}

    //private void Update()
    //{
        //if (rayHit)
        //{
        //    if (rayHitObject.transform.gameObject == target.parent.gameObject || rayHitObject.transform.gameObject.CompareTag("Tower")) 
        //        enemyIsForward = true;
        //    else enemyIsForward = false;
        //}
        //else enemyIsForward = false;
        //Stop_or_Move();
    //}

    public override void UnitTypeMove()
    {
        // 정지조건 3개
        if ((enemyIsForward && enemyDistance < stopDistanc) || (enemyDistance < 2f && !enemyIsForward) || 
            (Check_EnemyToUnit_Deggre() < 0.6f && enemyIsForward) || (enemyIsForward && target.gameObject.tag == "Tower"))
        {
            nav.isStopped = true;
        }
        else nav.isStopped = false;
    }

    protected float Check_EnemyToUnit_Deggre()
    {
        Enemy enemy = GetEnemyScript();
        if (enemy.gameObject.tag == "Tower") return 1f;

        float enemyDot = Vector3.Dot(enemy.dir.normalized, (target.position - this.transform.position).normalized);
        return enemyDot;
    }

    protected void HitMeeleAttack() // 근접공격 타겟팅, damage는 델리게이트 통일을 위한 잉여 변수
    {
        // 공격 시작 때 적과 HitMeeleAttack() 작동 시 적과 같은 적인지 비교하는 코드 필요
        Enemy enemy = GetEnemyScript();
        if (enemy != null && (enemyDistance < attackRange || target.gameObject.CompareTag("Tower")))
        {
            enemy.OnDamage(this.damage, teamSoldier);
        }
    }
}
