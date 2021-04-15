using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : TeamSoldier
{
    private Animator animator;
    public BoxCollider swordCollider;
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
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.5f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        swordCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        swordCollider.enabled = false;
        trail.SetActive(false);
    }
}
