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
        skillDamage = (int)(damage * 1.5f);
    }

    public override void SetPassiveFigure()
    {
        redPassiveFigure = 0.2f;
        bluePassiveFigure = new Vector2(70 , 3);
        yellowPassiveFigure = new Vector2(10 , 1);
        greenPassiveFigure = 2f;
        orangePassiveFigure = 3.5f;
        violetPassiveFigure = new Vector3(50, 3, 3000);
    }

    public override void NormalAttack()
    {
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

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

        isSkillAttack = false;
        base.NormalAttack();
    }

    // 이벤트

    // 스킬 사용 빈도 증가
    public void SkillPercentUp()
    {
        specialAttackPercent += 30;
    }

    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 0.1f;
        bluePassiveFigure = new Vector2(80, 4);
        yellowPassiveFigure = new Vector2(20, 1);
        greenPassiveFigure = 3f;
        orangePassiveFigure = 5;
        violetPassiveFigure = new Vector3(70, 4, 6000);
    }

    public void HitThrowWeapon(Enemy enemy, AttackWeapon attackWeapon)
    {
        Hit_Passive(enemy);

        if (attackWeapon.isSkill) enemy.OnDamage(skillDamage);
        else AttackEnemy(enemy);
    }
}
