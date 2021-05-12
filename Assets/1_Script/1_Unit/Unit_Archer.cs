using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Archer : RangeUnit
{
    private GameObject trail;
    public GameObject arrow;
    public Transform arrowTransform;

    private void Awake()
    {
        if(!enterStoryWorld) trail = GetComponentInChildren<TrailRenderer>().gameObject;
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                attackDelayTime *= 0.5f;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                speed *= 2;
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("ArrowAttack");
    }

    IEnumerator ArrowAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        trail.SetActive(false);
        GameObject instantArrow = CreateBullte(arrow, arrowTransform);
        ShotBullet(instantArrow, 2f, 50f);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.angularSpeed = 1000;

        isAttack = false;
        base.NormalAttack();
    }

    public override void RangeUnit_PassiveAttack()
    {
        Enemy enemy = GetEnemyScript();
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(30);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(1, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(5, 2);
                break;
        }
    }
}


// 스킬 코드
//NavMeshAgent arrowNav = instantArrow.GetComponent<NavMeshAgent>();
//    while (instantArrow != null)
//    {
//        arrowNav.SetDestination(target.position);
//        yield return new WaitForSeconds(0.08f);
//    }
