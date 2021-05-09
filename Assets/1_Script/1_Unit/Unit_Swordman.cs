using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : MeeleUnit
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
        base.NormalAttack();
        StartCoroutine(SwordAttack());
    }

    IEnumerator SwordAttack()
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (target != null && (enemyDistance < attackRange || target.gameObject.CompareTag("Tower")))
        {
            HitMeeleAttack();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    EnemySlow(10);
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    StartCoroutine(PoisonAttack(3, 5, 0.3f));
                    break;
            }
        }
        trail.SetActive(false);
    }
}
