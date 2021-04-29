using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : TeamSoldier
{
    // trail 시간 0.5로 그래프 0.5시작 끝 0

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
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(0.3f);
        if (Check_EnemyToUnit_Deg() < 0f)
        {
            nav.angularSpeed = 0.1f;
            nav.speed = 0.1f;
        }
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        nav.angularSpeed = 1500;
        nav.speed = this.speed;
        trail.SetActive(false);
    }
}
