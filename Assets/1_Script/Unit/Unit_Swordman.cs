﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : TeamSoldier
{
    public Animator animator;
    public BoxCollider swordCollider;
    public GameObject trail;

    private void Awake()
    {
        attackDelayTime = 2f;
        attackRange = 6f;
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
