using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : TeamSoldier
{
    private Animator animator;
    public BoxCollider spearCollider;
    public GameObject trail;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speed = 20f;
        attackDelayTime = 2.5f;
        attackRange = 3f;
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        StartCoroutine(SwordAttack());
    }

    IEnumerator SwordAttack()
    {
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.75f);
        trail.SetActive(true);
        spearCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        spearCollider.enabled = false;
        trail.SetActive(false);
    }
}
