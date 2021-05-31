using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class MageSpecialAttacks
//{
//    public GameObject posionEffect;
    
//}

public class Unit_Mage : RangeUnit, IUnitMana
{
    [Header("메이지 변수")]
    public GameObject magicLight;
    private Animator animator;

    public GameObject energyBall;
    public Transform energyBallTransform;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (unitColor == UnitColor.white) return;
        canvasRectTransform = transform.parent.GetComponentInChildren<RectTransform>();
        manaSlider = transform.parent.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
    }

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
                attackRange *= 2;
                break;
            case UnitColor.orange:
                bossDamage *= 6;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        //if (currentMana < maxMana) StartCoroutine("MageAttack");
        //else MageSpecialAttack();
        StartCoroutine("MageAttack");
    }

    public override void SpecialAttack()
    {
        MageSpecialAttack();
    }

    public int plusMana = 30;
    public float mageSkillCoolDownTime;
    IEnumerator MageAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        AddMana(plusMana);
        if (currentMana >= maxMana) specialAttackPercent = 100; // 이번 공격 때 마나 채워지면 다음 공격은 스킬
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.angularSpeed = 1000;

        isAttack = false;
        base.NormalAttack();
        if (enemySpawn.currentEnemyList.Count != 0 && !target.gameObject.CompareTag("Tower") && !target.gameObject.CompareTag("Boss")) UpdateTarget();
    }

    void MageSpecialAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;
        //Debug.Log("특별하다!!!!!!");
        
        MageColorSpecialAttack();
        ClearMana();

        isAttack = false;
        // 스킬 쿨타임 적용
        float saveAttackDelayTime = attackDelayTime;
        attackDelayTime = mageSkillCoolDownTime;
        base.NormalAttack();
        attackDelayTime = saveAttackDelayTime;
    }

    public GameObject mageEffectObject = null;
    void ShowMageSkillEffect(GameObject effectObject) // 특정 색깔은 스킬이 effect에 콜라이더에 맞겨져서 이거 자체가 스킬임
    {
        if (effectObject == null) return;

        effectObject.SetActive(true);
    }

    void MageColorSpecialAttack() // 색깔에 따른 실질적인 스킬
    {
        switch (unitColor)
        {
            case UnitColor.red:
                RedMageSkill();
                break;
            case UnitColor.blue:
                BlueMageSkill();
                break;
            case UnitColor.yellow:
                YellowMageSkill(5);
                break;
            case UnitColor.green:
                GreenMageSkill();
                break;
            case UnitColor.orange:
                OrangeMageSkill();
                break;
            case UnitColor.violet:
                VioletMageSkill(target);
                break;
            case UnitColor.black:
                BlackMageSkill();
                break;
        }
    }

    public AudioClip mageSkillCilp;

    IEnumerator Play_SkillClip(AudioClip playClip, float audioSound, float audioDelay)
    {
        yield return new WaitForSeconds(audioDelay);
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(playClip, audioSound);
    }

    void RedMageSkill() // 메테오 떨어뜨림
    {
        Vector3 meteorPosition = transform.position + Vector3.up * 60;
        GameObject instantSkillEffect = Instantiate(mageEffectObject, meteorPosition, Quaternion.identity);
        instantSkillEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f));
    }

    void BlueMageSkill()
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(Play_SkillClip(mageSkillCilp, 3f, 0.1f));
    }

    public GameObject posionEffect;
    void VioletMageSkill(Transform attackTarget) // 독 공격
    {
        GameObject instantPosionEffect = Instantiate(posionEffect, attackTarget.position, posionEffect.transform.rotation);
        instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1.5f, 0));
    }

    void YellowMageSkill(int addGold) // 골드 증가
    {
        StartCoroutine(Play_SkillClip(mageSkillCilp, 7f, 1f));
        ShowMageSkillEffect(mageEffectObject);
        GameManager.instance.Gold += addGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void GreenMageSkill()
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(GreenMageSkile_Coroutine());
        StartCoroutine(Play_SkillClip(mageSkillCilp, 2f, 0));
    }
    IEnumerator GreenMageSkile_Coroutine()
    {
        plusMana = 0;
        damage *= 5;
        yield return new WaitForSeconds(5f);
        damage /= 5;
        plusMana = 30;
    }

    void OrangeMageSkill()
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(OrangeMageSkile_Coroutine());
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0));
    }

    IEnumerator OrangeMageSkile_Coroutine()
    {
        plusMana = 0;
        attackDelayTime *= 0.2f;
        yield return new WaitForSeconds(10f);
        attackDelayTime *= 5;
        plusMana = 30;
    }

    void BlackMageSkill() // 사운드 넣어야 됨
    {
        Transform skillTransform = transform.GetChild(1);

        for(int i = 0; i < skillTransform.childCount; i++)
        {
            Transform instantTransform = skillTransform.GetChild(i);
            if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
            {
                GameObject instantEnergyBall = CreateBullte(energyBall, instantTransform);
                instantEnergyBall.transform.rotation = skillTransform.GetChild(i).rotation;
                instantEnergyBall.GetComponent<Rigidbody>().velocity = skillTransform.GetChild(i).rotation.normalized * Vector3.forward * 50;
            }
        }
    }

    public RectTransform canvasRectTransform;
    public Slider manaSlider;
    public int maxMana;
    public int currentMana;

    public void SetCanvas()
    {
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
        canvasRectTransform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    public void AddMana(int addMana)
    {
        if (unitColor == UnitColor.white) return;
        currentMana += addMana;
        manaSlider.value = currentMana;
    }

    public void ClearMana()
    {
        currentMana = 0;
        manaSlider.value = 0;
        specialAttackPercent = 0;
    }

    public override void RangeUnit_PassiveAttack(Enemy enemy)
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
                break;
            case UnitColor.orange:
                break;
            case UnitColor.violet:
                enemy.EnemyStern(60, 3);
                break;
        }
    }


    // 충돌 관련 패시브

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == 8)
        //{
        //    Enemy enemy = other.gameObject.GetComponent<Enemy>();
        //    switch (unitColor)
        //    {
        //        case UnitColor.red:
        //            break;
        //        case UnitColor.blue:
        //            break;
        //        case UnitColor.yellow:
        //            break;
        //        case UnitColor.green:
        //            break;
        //        case UnitColor.orange:
        //            break;
        //        case UnitColor.violet:
        //            break;
        //    }
        //}

        if(other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage *= 2;
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 0.5f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.EnemySlow(50, -1f); // 나가기 전까진 무한 슬로우
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.red:
                    break;
                case UnitColor.blue:
                    enemy.ExitSlow();
                    break;
                case UnitColor.yellow:
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }

        if (other.gameObject.layer == 9)
        {
            //Debug.Log("Unit ㅃㅃ");
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage = Mathf.RoundToInt(otherTeamSoldier.damage / 1.5f);
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 2f;
                    break;
                case UnitColor.green:
                    break;
                case UnitColor.orange:
                    break;
                case UnitColor.violet:
                    break;
            }
        }
    }

}