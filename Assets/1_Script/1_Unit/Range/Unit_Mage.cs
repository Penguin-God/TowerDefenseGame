using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Mage : RangeUnit, IEvent
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

        SetSkileObject();

        canvasRectTransform = transform.parent.GetComponentInChildren<RectTransform>();
        manaSlider = transform.parent.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        StartCoroutine(Co_SetCanvas());

        SetMagePassiveFigure();
        SetMagePassive();
        OnAwake();
    }

    public virtual void SetSkileObject()
    {
        mageSkill =
            Instantiate(mageEffectObject, mageEffectObject.transform.position, mageEffectObject.transform.rotation).GetComponent<MageSkill>();
    }

    public virtual void OnAwake() {}

    public void SetMagePassive()
    {
        switch (unitColor)
        {
            case UnitColor.red:
                GetComponent<SphereCollider>().radius = redPassiveFigure;
                break;
            case UnitColor.blue:
                GetComponent<SphereCollider>().radius = bluePassiveFigure.y;
                break;
            case UnitColor.green:
                attackRange *= 2;
                damage += Mathf.FloorToInt( ( greenPassiveFigure - 1) * originDamage);
                break;
            case UnitColor.orange:
                bossDamage += Mathf.FloorToInt( ( orangePassiveFigure - 1 ) * originBossDamage);
                break;
        }
    }

    // 충돌 관련 패시브
    private void OnTriggerEnter(Collider other)
    {
        // 적에 닿는건 없음

        if (other.gameObject.layer == 9)
        {
            TeamSoldier otherTeamSoldier = other.gameObject.GetComponent<TeamSoldier>();
            switch (unitColor)
            {
                case UnitColor.red:
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
                    enemy.EnemySlow(bluePassiveFigure.y, -1f); // 나가기 전까진 무한 슬로우
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
                    otherTeamSoldier.attackDelayTime *= 2f;
                    break;
            }
        }
    }

    public void SetMagePassiveFigure()
    {
        redPassiveFigure = 15f;
        bluePassiveFigure = new Vector2(60, 25); // x는 슬로우 정도 y는 콜라이더 범위
        yellowPassiveFigure = new Vector2(15, 2);
        greenPassiveFigure = 3f;
        orangePassiveFigure = 5f;
        violetPassiveFigure = new Vector3(60, 5, 60000);
    }


    // 평타강화 함수
    delegate void AttackDelegate();
    AttackDelegate attackReinforceDelegate = null;

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
    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        AddMana(plusMana);
        if (currentMana >= maxMana) specialAttackPercent = 100; // 이번 공격 때 마나 채워지면 다음 공격은 스킬확률을 100퍼로 해서 무조건 스킬 씀
        magicLight.SetActive(true);

        if (target != null && enemyDistance < chaseRange)
        {
            GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform, delegate_OnHit);
            ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        // 평타 강화 시 작동 (대미지가 아닌 3방향 공격 같은 효과 강화)
        if (attackReinforceDelegate != null) attackReinforceDelegate();

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.NormalAttack();
    }

    void MageSpecialAttack()
    {
        isSkillAttack = true;

        MageColorSpecialAttack();
        ClearMana();

        // 스킬 쿨타임 적용
        StartCoroutine(Co_SkillCoolDown());
    }

    IEnumerator Co_SkillCoolDown()
    {
        yield return new WaitForSeconds(mageSkillCoolDownTime);
        isSkillAttack = false;
    }

    public GameObject mageEffectObject = null;
    void ShowMageSkillEffect(GameObject effectObject) // 특정 색깔은 스킬이 effect에 콜라이더에 맞겨져서 이거 자체가 스킬임
    {
        if (effectObject == null) return;

        effectObject.SetActive(true);
    }

    protected void SetSkilObject(Vector3 _position)
    {
        mageSkill.transform.position = _position;
        mageSkill.gameObject.SetActive(true);
    }

    protected MageSkill mageSkill = null;
    void MageColorSpecialAttack() // 색깔에 따른 실질적인 스킬
    {
        MageSkile();
        //if (mageSkill != null) mageSkill.OnSkile(this);
        //else Debug.Log("MageSkile 스크립트 없음");
    }

    protected void PlaySkileAudioClip()
    {
        switch (unitColor)
        {
            case UnitColor.red: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0f));  break;
            case UnitColor.blue: StartCoroutine(Play_SkillClip(mageSkillCilp, 3f, 0.1f)); break;
            case UnitColor.yellow: StartCoroutine(Play_SkillClip(mageSkillCilp, 7f, 0.7f)); break;
            case UnitColor.green: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f)); break;
            case UnitColor.orange: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f)); break;
            case UnitColor.violet: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f)); break;
            case UnitColor.black: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0.7f)); break;
        }
    }

    public AudioClip mageSkillCilp;

    protected IEnumerator Play_SkillClip(AudioClip playClip, float audioSound, float audioDelay)
    {
        yield return new WaitForSeconds(audioDelay);
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(playClip, audioSound);
    }

    void RedMageSkill()
    {
        RedMageSkillAttack(target);
        if (isUltimate && enemySpawn.currentEnemyList.Count > 1) RedMageSkillAttack(Return_RandomCurrentEnemy(1)[0]);
    }
    void RedMageSkillAttack(Transform attackTarget) // 메테오 떨어뜨림
    {
        if (attackTarget == null) return;

        Vector3 meteorPosition = transform.position + Vector3.up * 30; // 높이 설정
        GameObject instantSkillEffect = Instantiate(mageEffectObject, meteorPosition, Quaternion.identity);

        instantSkillEffect.SetActive(true);
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


    void GreenMageSkill()
    {
        StartCoroutine(GreenMageSkile_Coroutine());
        StartCoroutine(Play_SkillClip(normalAttackClip, 1f, 0.7f));
    }
    IEnumerator GreenMageSkile_Coroutine()
    {
        if (isUltimate) attackReinforceDelegate += MultiDirectionAttack;
        GameObject saveEnergyball = energyBall;
        energyBall = mageEffectObject;
        int savePlusMana = plusMana;
        plusMana = 0;

        StartCoroutine("MageAttack");
        yield return new WaitUntil(() => !isAttackDelayTime);

        plusMana = savePlusMana;
        energyBall = saveEnergyball;
        if (attackReinforceDelegate != null) attackReinforceDelegate -= MultiDirectionAttack;
    }

    void OrangeMageSkill() // 애는 스킬 쪽에서 소리 재생
    {
        GameObject instantPosionEffect = Instantiate(mageEffectObject, target.position, mageEffectObject.transform.rotation);
        //instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
    }

    void MultiDirectionAttack()
    {
        Transform directions = transform.GetChild(2);
        for (int i = 0; i < directions.childCount; i++)
        {
            Transform instantTransform = directions.GetChild(i);
            if (target != null && Vector3.Distance(target.position, transform.position) < chaseRange)
            {
                GameObject instantEnergyBall = CreateBullte(energyBall, instantTransform, delegate_OnHit);
                instantEnergyBall.transform.rotation = directions.GetChild(i).rotation;
                instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
            }
        }
    }


    void VioletMageSkill(Transform attackTarget) // 독 공격
    {
        GameObject instantPosionEffect = Instantiate(mageEffectObject, attackTarget.position, mageEffectObject.transform.rotation);
        //instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
        if (isUltimate) Ultimate_VioletMageSkill();
        StartCoroutine(Play_SkillClip(mageSkillCilp, 1.5f, 0));
    }

    void Ultimate_VioletMageSkill()
    {
        if (enemySpawn.currentEnemyList.Count <= 1) return;

        Transform target = Return_RandomCurrentEnemy(1)[0];
        GameObject instantPosionEffect = Instantiate(mageEffectObject, target.position, mageEffectObject.transform.rotation);
        //instantPosionEffect.GetComponent<MageSkill>().teamSoldier = this.GetComponent<TeamSoldier>();
    }

    void BlackMageSkill()
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
                GameObject instantEnergyBall = CreateBullte(energyBall, instantTransform, delegate_OnHit);
                instantEnergyBall.transform.rotation = directions.GetChild(i).rotation;
                instantEnergyBall.GetComponent<Rigidbody>().velocity = directions.GetChild(i).rotation.normalized * Vector3.forward * 50;
                instantEnergyBall.GetComponent<AttackWeapon>().isSkill = true;
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

    private RectTransform canvasRectTransform;
    private Slider manaSlider;
    public int maxMana;
    public int currentMana;

    IEnumerator Co_SetCanvas()
    {
        if (unitColor == UnitColor.white) yield break;
        while (true)
        {
            canvasRectTransform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            yield return null;
        }
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


    // 이벤트

    // 스킬 빈도 증가
    public void SkillPercentUp()
    {
        plusMana += 20;
    }

    // 패시브 강화
    public void ReinforcePassive()
    {
        redPassiveFigure = 0.3f;
        bluePassiveFigure = new Vector2(60, 40);
        yellowPassiveFigure = new Vector2(30, 2);
        greenPassiveFigure = 5.5f;
        orangePassiveFigure = 8.5f;
        violetPassiveFigure = new Vector3(90, 6, 120000);
    }

    public virtual void MageSkile() 
    {
        isSkillAttack = true;
        ClearMana();
        StartCoroutine(Co_SkillCoolDown());
        PlaySkileAudioClip();
    }
}