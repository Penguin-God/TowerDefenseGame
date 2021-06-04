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

    private float redPassiveFigure = 0.5f;
    private int bluePassiveFigure = 10;
    private float yellowPassiveFigure = 1; // 나중에 익준이 스크립트에 적용
    private int greenPassiveFigure = 5;
    private float orangePassiveFigure = 1.5f;
    private int violetPassiveFigure = 10;

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                attackDelayTime *= redPassiveFigure;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                speed += greenPassiveFigure;
                break;
            case UnitColor.orange:
                damage = Mathf.RoundToInt(damage * orangePassiveFigure);
                break;
            case UnitColor.violet:
                break;
        }
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
        //if(audioSource != null) audioSource.Play();
        yield return new WaitForSeconds(0.3f);
        if (target != null && (enemyDistance < attackRange || target.gameObject.CompareTag("Tower")))
        {
            HitMeeleAttack();
        }
        trail.SetActive(false);

        isAttack = false;
        base.NormalAttack();
    }

    public override void MeeleUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(bluePassiveFigure, 1);
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                SowrdPoisonAttack(enemy);
                break;
        }
    }

    private bool isPoisonAttack = false;
    void SowrdPoisonAttack(Enemy enemy)
    {
        if (isPoisonAttack) return;
        enemy.EnemyPoisonAttack(2, 5, 0.3f, violetPassiveFigure);
        Invoke("ExitPoisonAttack", 1.5f);
    }
    void ExitPoisonAttack()
    {
        isPoisonAttack = false;
    }

    // 이벤트
    public void SkillPercentUp() {}

    public void SkillPercentDown() {}

    public void ReinforcePassive()
    {
        redPassiveFigure = 0.25f;
        bluePassiveFigure = 20;
        yellowPassiveFigure = 2;
        greenPassiveFigure = 6;
        orangePassiveFigure = 2f;
        violetPassiveFigure = 30;
    }

    public void WeakenPassive()
    {
        redPassiveFigure = 0;
        bluePassiveFigure = 0;
        yellowPassiveFigure = 0;
        greenPassiveFigure = 0;
        orangePassiveFigure = 0;
        violetPassiveFigure = 0;
    }
}
