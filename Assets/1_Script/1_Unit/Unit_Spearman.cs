using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit
{
    private Animator animator;
    public GameObject trail;
    public GameObject spear;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Spearman_SpecialAttack());
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
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        isAttack = false;
        base.NormalAttack();
    }

    IEnumerator Spearman_SpecialAttack()
    {
        //nav.isStopped = true;

        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        trail.SetActive(true);
        spear.GetComponent<BoxCollider>().enabled = true;
        spear.GetComponent<Rigidbody>().velocity = Vector3.forward * 30;
        //Debug.Log(spear.GetComponent<Rigidbody>().velocity);
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
