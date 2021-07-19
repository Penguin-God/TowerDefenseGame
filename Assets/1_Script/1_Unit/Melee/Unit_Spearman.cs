using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit, IEvent, IHitThrowWeapon
{
    [Header("창병 변수")]
    public GameObject trail;
    public GameObject spear;
    public GameObject skileSpaer; // 발사할 때 생성하는 창
    public Transform spearCreatePosition;
    public GameObject dontMoveGameObject;

    [SerializeField]
    private AudioClip skillAudioClip;
    private Animator animator;

    private void Awake()
    {
        dontMoveGameObject = GameObject.Find("World");
        animator = GetComponent<Animator>();
    }

    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                specialAttackPercent = redPassiveFigure;
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                damage += (greenPassiveFigure - 1) * originDamage;
                break;
            case UnitColor.orange:
                bossDamage += (orangePassiveFigure - 1) * originBossDamage;
                break;
            case UnitColor.violet:
                break;
        }

        skillDamage = (int)(damage * 1.5f);
    }

    public override void NormalAttack()
    {
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        isAttackDelayTime = true;
        isAttack = true;

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        isAttack = false;
        base.NormalAttack();
    }

    public override void SpecialAttack()
    {
        StartCoroutine("Spearman_SpecialAttack");
    }

    IEnumerator Spearman_SpecialAttack()
    {
        isAttack = true;
        isSkillAttack = true;
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;

        GameObject instantSpear = Instantiate(skileSpaer, spearCreatePosition);
        instantSpear.transform.SetParent(dontMoveGameObject.transform);
        instantSpear.GetComponent<AttackWeapon>().attackUnit = this.gameObject;
        instantSpear.GetComponent<Rigidbody>().velocity = (-1 * transform.forward) * 50;

        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.12f);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);
        isAttack = false;
        isSkillAttack = false;
        base.NormalAttack();
    }

    

    public override void MeeleUnit_PassiveAttack(Enemy enemy)
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                enemy.EnemySlow(bluePassiveFigure, 3);
                break;
            case UnitColor.yellow:
                Add_PassiveGold(yellowPassiveFigure, 2);
                break;
            case UnitColor.green:
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(violetPassiveFigure, 5);
                break;
        }
    }

    // 이벤트

    // 스킬 사용 빈도 증가
    public void SkillPercentUp()
    {
        specialAttackPercent += 30;
    }
    // 패시브 관련 수치
    private int redPassiveFigure = 80;
    private int bluePassiveFigure = 50;
    private int yellowPassiveFigure = 5;
    private int greenPassiveFigure = 3;
    private int orangePassiveFigure = 3;
    private int violetPassiveFigure = 30;
    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 100;
        bluePassiveFigure = 85;
        yellowPassiveFigure = 15;
        greenPassiveFigure = 5;
        orangePassiveFigure = 5;
        violetPassiveFigure = 60;
    }

    public void HitThrowWeapon(Enemy enemy, AttackWeapon attackWeapon)
    {
        MeeleUnit_PassiveAttack(enemy);

        if (attackWeapon.isSkill) enemy.OnDamage(skillDamage);
        else AttackEnemy(enemy);
    }
}
