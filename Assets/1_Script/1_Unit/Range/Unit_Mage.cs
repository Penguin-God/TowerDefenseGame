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
    private Animator animator;
    public GameObject magicLight;

    public GameObject energyBall;
    public Transform energyBallTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
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
        if (currentMana < maxMana) StartCoroutine("MageAttack");
        else MageSpecialAttack();
    }

    public int plusMana = 30;
    IEnumerator MageAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        nav.angularSpeed = 1;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        AddMana(plusMana);
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < 150f)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }
        if (audioSource != null) audioSource.Play();

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.angularSpeed = 1000;
        
        isAttack = false;
        base.NormalAttack();
    }

    void MageSpecialAttack()
    {
        isAttack = true;
        isAttackDelayTime = true;

        Debug.Log("특별하다!!!!!!");
        ShowMageSkillEffect(mageEffectObject);
        MageColorSpecialAttack();
        ClearMana();

        isAttack = false;
        base.NormalAttack();
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
                //BlueMageSkill(30f);
                break;
            case UnitColor.yellow:
                YellowMageSkill(2);
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
        }
    }

    void RedMageSkill() // 메테오 떨어뜨림
    {
        GameObject instantSkillEffect = Instantiate(mageEffectObject, mageEffectObject.transform.position, Quaternion.identity);
        instantSkillEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
    }

    public GameObject posionEffect;
    void VioletMageSkill(Transform attackTarget) // 독 공격
    {
        GameObject instantPosionEffect = Instantiate(posionEffect, attackTarget.position, posionEffect.transform.rotation);
        instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
    }

    void YellowMageSkill(int addGold) // 골드 증가
    {
        GameManager.instance.Gold += addGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }

    void GreenMageSkill()
    {
        StartCoroutine(GreenMageSkile_Coroutine());
    }
    IEnumerator GreenMageSkile_Coroutine()
    {
        plusMana = 0;

        // 스킬 사용 후 바로 공격하게 하기 위한 코드
        attackDelayTime = 0;
        yield return new WaitUntil(() => !isAttackDelayTime); 
        attackDelayTime = 2f;

        damage *= 5;
        yield return new WaitForSeconds(5f);
        damage /= 5;
        plusMana = 30;
    }

    void OrangeMageSkill()
    {
        StartCoroutine(OrangeMageSkile_Coroutine());
    }

    IEnumerator OrangeMageSkile_Coroutine()
    {
        plusMana = 0;
        attackDelayTime *= 0.2f;
        yield return new WaitForSeconds(10f);
        attackDelayTime *= 5;
        plusMana = 30;
    }

    //void BlueMageSkill(float slowRange)
    //{
    //    List<GameObject> slowTargetList = Get_SlowTarget(slowRange);
    //    if (slowTargetList.Count == 0) return;

    //    for (int i = 0; i < slowTargetList.Count; i++)
    //    {
    //        slowTargetList[i].GetComponent<Enemy>().EnemySlow(99);
    //    }
    //}
    //List<GameObject> Get_SlowTarget(float slowRange) // 거리 안에 있는 enemy들을 List로 가져옴
    //{
    //    List<GameObject> slowTargetEnemyList = new List<GameObject>();
    //    foreach (GameObject enemyObject in enemySpawn.currentEnemyList)
    //    {
    //        if (enemyObject != null)
    //        {
    //            float distanceToEnemy = Vector3.Distance(this.transform.position, enemyObject.transform.position);
    //            if (distanceToEnemy < slowRange)
    //            {
    //                slowTargetEnemyList.Add(enemyObject);
    //            }
    //        }
    //    }

    //    return slowTargetEnemyList;
    //}



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
        if (unitColor == UnitColor.black || unitColor == UnitColor.white) return;
        currentMana += addMana;
        manaSlider.value = currentMana;
    }

    public void ClearMana()
    {
        currentMana = 0;
        manaSlider.value = 0;
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

    private void OnTriggerEnter(Collider other)
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
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage /= 2;
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