using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Mage : RangeUnit, IUnitMana, IEvent
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
                GetComponent<SphereCollider>().radius = bluePassiveFigure;
                break;
            case UnitColor.yellow:
                GetComponent<SphereCollider>().radius = yellowPassiveFigure;
                break;
            case UnitColor.green:
                attackRange *= 2;
                damage += (greenPassiveFigure - 1) * originDamage;
                break;
            case UnitColor.orange:
                bossDamage += (orangePassiveFigure - 1) * originBossDamage;
                break;
            case UnitColor.violet:
                break;
        }
    }

    public override void NormalAttack()
    {
        StartCoroutine("MageAttack");
    }

    public bool isUltimate; // 스킬 강화
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

        //nav.angularSpeed = 1;
        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        AddMana(plusMana);
        if (currentMana >= maxMana) specialAttackPercent = 100; // 이번 공격 때 마나 채워지면 다음 공격은 스킬확률을 100퍼로 해서 무조건 스킬 씀
        magicLight.SetActive(true);

        if (target != null && Vector3.Distance(target.position, transform.position) < chaseRange)
        {
            if (unitColor == UnitColor.orange && isUltimate && isOrangeSkill) MultiDirectionAttack(transform.GetChild(2));
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        isAttack = false;
        base.NormalAttack();
        //if (enemySpawn.currentEnemyList.Count != 0 && !target.gameObject.CompareTag("Tower") && !target.gameObject.CompareTag("Boss")) UpdateTarget();
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
                int addGold = (isUltimate) ? 3 : 5;
                YellowMageSkill(addGold);
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

    void RedMageSkill()
    {
        RedMageSkillAttack(target);
        if (isUltimate) RedMageSkillAttack(Return_RandomCurrentEnemy(1)[0]);
    }
    void RedMageSkillAttack(Transform attackTarget) // 메테오 떨어뜨림
    {
        Vector3 meteorPosition = transform.position + Vector3.up * 30; // 높이 설정
        GameObject instantSkillEffect = Instantiate(mageEffectObject, meteorPosition, Quaternion.identity);
        MageSkill mageSkill = instantSkillEffect.GetComponent<MageSkill>();
        mageSkill.target = attackTarget;
        mageSkill.teamSoldier = GetComponent<TeamSoldier>();
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f));
    }


    void BlueMageSkill()
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(Play_SkillClip(mageSkillCilp, 3f, 0.1f));
    }


    void YellowMageSkill(int addGold) // 골드 증가
    {
        StartCoroutine(Play_SkillClip(mageSkillCilp, 7f, 1f));
        ShowMageSkillEffect(mageEffectObject);
        GameManager.instance.Gold += addGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }


    void GreenMageSkill() // 대미지 5배
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(GreenMageSkile_Coroutine());
        StartCoroutine(Play_SkillClip(mageSkillCilp, 2f, 0));
    }
    IEnumerator GreenMageSkile_Coroutine()
    {
        int originPlusMana = plusMana;
        plusMana = 0;
        int addDamageRate = (isUltimate) ? 8 : 5;
        damage += addDamageRate * originDamage;
        yield return new WaitForSeconds(5f);
        damage -= addDamageRate * originDamage;
        plusMana = originPlusMana;
    }

    private bool isOrangeSkill = false;
    void OrangeMageSkill() // 공속 5배
    {
        ShowMageSkillEffect(mageEffectObject);
        StartCoroutine(OrangeMageSkile_Coroutine());
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0));
    }

    IEnumerator OrangeMageSkile_Coroutine()
    {
        isOrangeSkill = true;
        int originPlusMana = plusMana;
        plusMana = 0;
        attackDelayTime *= 0.2f;
        yield return new WaitForSeconds(10f);
        attackDelayTime *= 5;
        plusMana = originPlusMana;
        isOrangeSkill = false;
    }

    void VioletMageSkill(Transform attackTarget) // 독 공격
    {
        GameObject instantPosionEffect = Instantiate(mageEffectObject, attackTarget.position, mageEffectObject.transform.rotation);
        instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
        if (isUltimate) Ultimate_VioletMageSkill();
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1.5f, 0));
    }

    void Ultimate_VioletMageSkill()
    {
        if (enemySpawn.currentEnemyList.Count <= 1) return;

        Transform target = Return_RandomCurrentEnemy(1)[0];
        GameObject instantPosionEffect = Instantiate(mageEffectObject, target.position, mageEffectObject.transform.rotation);
        instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
    }

    void BlackMageSkill() // 사운드 넣어야 됨
    {
        int chiledNumber = (isUltimate) ? 2 : 1;
        Transform skillTransform = transform.GetChild(chiledNumber); // 자식 가져옴

        MultiDirectionAttack(skillTransform);
        StartCoroutine(Play_SkillClip(mageSkillCilp, 0.7f, 0));
    }

    void MultiDirectionAttack(Transform directions)
    {
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);
            if (target != null && Vector3.Distance(target.position, transform.position) < chaseRange)
            {
                GameObject instantEnergyBall = CreateBullte(energyBall, instantTransform);
                instantEnergyBall.transform.rotation = directions.GetChild(i).rotation;
                instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
            }
        }
    }

    Transform[] Return_RandomCurrentEnemy(int enemyCount) // enemyCount만큼의 적 트랜스폼 배열 반환
    {
        Transform[] enemys = new Transform[enemyCount];
        for(int i = 0; i < enemyCount; i++)
        {
            int random = Random.Range(0, enemySpawn.currentEnemyList.Count);
            enemys[i] = enemySpawn.currentEnemyList[random].transform;
        }
        return enemys;
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
            case UnitColor.violet:
                enemy.EnemyStern(violetPassiveFigure, 7);
                break;
        }
    }


    // 충돌 관련 패시브

    private void OnTriggerEnter(Collider other)
    {
        // 적에 닿는건 없음

        if(other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage += Mathf.RoundToInt( (redPassiveFigure - 1) * otherTeamSoldier.originDamage);
                    break;
                case UnitColor.blue:
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 0.5f;
                    break;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.blue:
                    enemy.EnemySlow(50, -1f); // 나가기 전까진 무한 슬로우
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<NomalEnemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            switch (unitColor)
            {
                case UnitColor.blue:
                    enemy.ExitSlow();
                    break;
            }
        }

        if (other.gameObject.layer == 9) // 유닛 버프
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
                    otherTeamSoldier.damage -= Mathf.RoundToInt((redPassiveFigure - 1) * otherTeamSoldier.originDamage);
                    break;
                case UnitColor.yellow:
                    otherTeamSoldier.attackDelayTime *= 2f;
                    break;
            }
        }
    }



    // 이벤트

    // 스킬 빈도 증가
    public void SkillPercentUp()
    {
        plusMana += 20;
    }

    //패시브 관련 변수
    private float redPassiveFigure = 1.5f;
    private float bluePassiveFigure = 25f;
    private float yellowPassiveFigure = 20f;
    private int greenPassiveFigure = 4;
    private int orangePassiveFigure = 5;
    private int violetPassiveFigure = 60;
    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 2.5f;
        bluePassiveFigure = 40f;
        yellowPassiveFigure = 40f;
        greenPassiveFigure = 7;
        orangePassiveFigure = 10;
        violetPassiveFigure = 100;
    }
}