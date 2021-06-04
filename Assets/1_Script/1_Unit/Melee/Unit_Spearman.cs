using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spearman : MeeleUnit, IEvent
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


    private float redPassiveFigure; // 애는 없음 
    private int bluePassiveFigure = 50;
    private int yellowPassiveFigure = 5;
    private int greenPassiveFigure = 3; 
    private int orangePassiveFigure = 3;
    private int violetPassiveFigure = 30;
    public override void SetPassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                break;
            case UnitColor.blue:
                break;
            case UnitColor.yellow:
                break;
            case UnitColor.green:
                damage *= greenPassiveFigure;
                break;
            case UnitColor.orange:
                bossDamage *= orangePassiveFigure;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("SpaerAttack");
    }

    IEnumerator SpaerAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

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
        //base.SpecialAttack(); // 나중에 스킬 쿨타임을 따로 만들수도 있음
        StartCoroutine("Spearman_SpecialAttack");
    }

    IEnumerator Spearman_SpecialAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;
        //Vector3 createRotation = new Vector3(-90f, transform.parent.rotation.y, spearCreatePosition.rotation.z);
        //Debug.Log(createRotation);
        GameObject instantSpear = Instantiate(skileSpaer, spearCreatePosition);
        instantSpear.transform.SetParent(dontMoveGameObject.transform);
        instantSpear.GetComponent<AttackWeapon>().attackUnit = this.gameObject;
        instantSpear.GetComponent<Rigidbody>().velocity = (-1 * transform.forward) * 50;
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(skillAudioClip, 0.15f);
        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);
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
                enemy.EnemyStern(violetPassiveFigure, 2);
                break;
        }
    }

    // 이벤트
    public void SkillPercentUp()
    {
        specialAttackPercent += 15;
    }

    public void SkillPercentDown()
    {
        specialAttackPercent -= 15;
    }

    public void ReinforcePassive()
    {
        redPassiveFigure = 0;
        bluePassiveFigure = 85;
        yellowPassiveFigure = 15;
        greenPassiveFigure = 5;
        orangePassiveFigure = 5;
        violetPassiveFigure = 60;
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
