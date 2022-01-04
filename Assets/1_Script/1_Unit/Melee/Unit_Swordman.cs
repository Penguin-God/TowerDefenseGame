using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : MeeleUnit, IEvent
{
    [Header("기사 변수")]
    public GameObject trail;

    public override void NormalAttack()
    {
        StartCoroutine("SwordAttack");
    }

    IEnumerator SwordAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isSword");
        yield return new WaitForSeconds(0.8f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        trail.SetActive(false);

        base.NormalAttack();
    }

    // 이벤트
    public void SkillPercentUp() {}
}
