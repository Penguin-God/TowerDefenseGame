using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : MeeleUnit, IEvent
{
    [Header("기사 변수")]
    public GameObject trail;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetPassiveFigure()
    {
        redPassiveFigure = 0.7f;
        bluePassiveFigure = new Vector2(10, 2);
        yellowPassiveFigure = Vector2.zero;
        greenPassiveFigure = 1.2f;
        orangePassiveFigure = 1.5f;
        violetPassiveFigure = new Vector3(10, 2, 8);
    }

    public override void NormalAttack()
    {
        StartCoroutine("SwordAttack");
    }

    IEnumerator SwordAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        
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

    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 0.5f;
        bluePassiveFigure = new Vector2(30, 2);
        yellowPassiveFigure = Vector2.zero;
        greenPassiveFigure = 1.5f;
        orangePassiveFigure = 2f;
        violetPassiveFigure = new Vector3(25, 2, 16);
    }
}
