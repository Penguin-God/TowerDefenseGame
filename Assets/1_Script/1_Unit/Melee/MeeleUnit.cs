using UnityEngine;

public class MeeleUnit : TeamSoldier
{
    Vector3 destinationPos = Vector3.zero;
    public override Vector3 DestinationPos => destinationPos;

    public override void UnitTypeMove()
    {
        if(enemyDistance < stopDistanc * 2 && Check_EnemyToUnit_Deggre() < -0.5f && enemyIsForward)
        {
            Debug.Log(1234);
            destinationPos = target.position - (TargetEnemy.dir * -3);
            nav.acceleration = 10f;
            nav.angularSpeed = 100;
            nav.speed = 0.5f;
        }
        else if (enemyDistance < stopDistanc)
        {
            destinationPos = target.position - (TargetEnemy.dir * 3);
            nav.acceleration = 0.5f;
            nav.angularSpeed = 500;
            nav.speed = 0.15f;
            contactEnemy = true;
        }
        else
        {
            destinationPos = target.position - (TargetEnemy.dir * 3);
            nav.speed = this.speed;
            nav.angularSpeed = 500;
            nav.acceleration = 40;
            contactEnemy = false;
        }
    }

    protected float Check_EnemyToUnit_Deggre()
    {
        if (target == null) return 1f;
        float enemyDot = Vector3.Dot(TargetEnemy.dir.normalized, (target.position - transform.position));
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

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.parent.position + Vector3.up, transform.parent.forward * attackRange, Color.green);
        rayHit = Physics.Raycast(transform.parent.position + Vector3.up, transform.parent.forward, out rayHitObject, attackRange, layerMask);
    }

}
