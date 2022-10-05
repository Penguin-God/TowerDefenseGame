using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_MeleeUnit : Multi_TeamSoldier
{
    Vector3 destinationPos = Vector3.zero;
    protected override Vector3 DestinationPos
    {
        get
        {
            if (EnterStroyWorld == false) return destinationPos;
            else return base.DestinationPos;
        }
    }

    public override void UnitTypeMove()
    {
        if (Check_EnemyToUnit_Deggre() < -0.8f && enemyDistance < 10)
        {
            if (enemyIsForward || _state.IsAttack)
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
            nav.speed = Speed;
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
        if (PhotonNetwork.IsMasterClient == false) return;

        if (target != null && enemyDistance < AttackRange * 2)
            OnHit?.Invoke(TargetEnemy);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * AttackRange, Color.green);
        rayHit = Physics.Raycast(transform.position + Vector3.up, transform.forward, out rayHitObject, AttackRange, layerMask);
    }
}
