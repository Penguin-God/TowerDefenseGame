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
}
