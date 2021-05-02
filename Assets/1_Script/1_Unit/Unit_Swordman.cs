using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : TeamSoldier
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
        StartCoroutine(SwordAttack());
    }

    IEnumerator SwordAttack()
    {
        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack(); // 공격 시작 때 적과 HitMeeleAttack() 작동 시 적과 같은 적인지 비교하는 코드 필요
        trail.SetActive(false);
    }
}
