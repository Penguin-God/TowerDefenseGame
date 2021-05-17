using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Mage : RangeUnit
{
    private Animator animator;
    public GameObject magicLight;

    public GameObject energyBall;
    public Transform energyBallTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                attackRange *= 2;
                break;
            case UnitColor.orange:
                bossDamage *= 6;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("MageAttack");
    }

    IEnumerator MageAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.6f);
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.angularSpeed = 1000;
        
        isAttack = false;
        base.NormalAttack();
    }

    public override void RangeUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(60, 3);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.EnemySlow(50);
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }

        if(other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage *= 2;
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 0.5f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.ExitSlow();
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }

        if (other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage /= 2;
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 2f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }


}
