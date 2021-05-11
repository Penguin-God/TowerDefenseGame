using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit
{
    private Animator animator;
    public GameObject trail;

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
                damage *= 3;
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);
    }

    public override void MeeleUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(50);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(5, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(30, 2);
                break;
        }
    }
}
