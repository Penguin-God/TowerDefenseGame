using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    Vector3 destinationPos = Vector3.zero;
    public override Vector3 DestinationPos => destinationPos;

    public override void UnitTypeMove()
    {
        if(Check_EnemyToUnit_Deggre() < - 0.8f && enemyDistance < 10)
        {
            if (enemyIsForward || isAttack)
            {
                destinationPos = target.position - (TargetEnemy.dir * -1f);
                nav.acceleration = 2f;
                nav.angularSpeed = 5;
                nav.speed = 1f;
            }
            else
            {
                destinationPos = target.position - (TargetEnemy.dir * -5f);
                nav.acceleration = 20f;
                nav.angularSpeed = 500;
                nav.speed = 15f;
            }
        }
        else if (enemyDistance < stopDistanc)
        {
            destinationPos = target.position - (TargetEnemy.dir * 2);
            nav.acceleration = 20f;
            nav.angularSpeed = 200;
            nav.speed = 5f;
            contactEnemy = true;
        }
        else
        {
            destinationPos = target.position - (TargetEnemy.dir * 1);
            nav.speed = this.speed;
            nav.angularSpeed = 500;
            nav.acceleration = 40;
            contactEnemy = false;
        }
    }

    protected float Check_EnemyToUnit_Deggre()
    {
        if (target == null) return 1f;
        float enemyDot = Vector3.Dot(TargetEnemy.dir.normalized, (destinationPos - transform.position));
        return enemyDot;
    }

    protected void HitMeeleAttack() // 근접공격 타겟팅
    {
        if (target != null && enemyDistance < attackRange * 2) 
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (delegate_OnHit != null) delegate_OnHit(enemy);
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up, transform.parent.forward, out rayHitObject, attackRange, layerMask);
    }

}
