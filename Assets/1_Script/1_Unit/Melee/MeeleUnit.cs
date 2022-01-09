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
        if (enemyDistance < stopDistanc)
        {
            nav.speed = 0.15f;
            contactEnemy = true;
        }
        else if((enemyDistance < stopDistanc * 2 && Check_EnemyToUnit_Deggre() < -0.5f))
        {
            if (enemyIsForward)
            {
                nav.speed = 0.01f;
                nav.angularSpeed = 1f;
            }
            else nav.speed = 0.2f;
        }
        else
        {
            nav.speed = this.speed;
            nav.angularSpeed = 500f;
            contactEnemy = false;
        }
    }

    protected float Check_EnemyToUnit_Deggre()
    {
        if (target == null) return 1f;
        float enemyDot = Vector3.Dot(TargetEnemy.dir.normalized, (target.position - this.transform.position).normalized);
        return enemyDot;
    }

    protected void HitMeeleAttack() // 근접공격 타겟팅
    {
        if (target != null && enemyDistance < attackRange * 1.5f) 
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (delegate_OnHit != null) delegate_OnHit(enemy);
        }
    }
}
