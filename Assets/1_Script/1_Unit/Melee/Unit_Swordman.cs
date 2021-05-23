using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Swordman : MeeleUnit
{
    [Header("기사 변수")]
    public GameObject trail;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                attackDelayTime *= 0.5f;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                speed += 5;
                break;
            case UnitColor.orange:
                damage = Mathf.RoundToInt(damage * 1.5f);
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
                enemy.EnemySlow(10, 1);
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
        enemy.EnemyPoisonAttack(2, 5, 0.3f, 10);
        Invoke("ExitPoisonAttack", 1.5f);
    }
    void ExitPoisonAttack()
    {
        isPoisonAttack = false;
    }
}
