using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : TeamSoldier
{
    private Animator animator;
    //public BoxCollider spearCollider;
    public GameObject trail;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(SwordAttack());
    }

    IEnumerator SwordAttack()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.45f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        trail.SetActive(false);
    }
}
