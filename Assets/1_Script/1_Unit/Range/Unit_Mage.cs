using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Mage : RangeUnit, IEvent
{
    [Header("메이지 변수")]
    public GameObject magicLight;

    public GameObject energyBall;
    public Transform energyBallTransform;

    public override void OnAwake()
    {
        SettingWeaponPool(energyBall, 7);
        if (unitColor == UnitColor.white) return;

        canvasRectTransform = transform.parent.GetComponentInChildren<RectTransform>();
        manaSlider = transform.parent.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        StartCoroutine(Co_SetCanvas());

        SetMageAwake();
    }

    public virtual void SetMageAwake()
    {
        mageEffectObject = Instantiate(mageEffectObject, mageEffectObject.transform.position, mageEffectObject.transform.rotation);
    }

    public override void NormalAttack()
    {
        StartCoroutine("MageAttack");
    }

    public bool isUltimate; // 스킬 강화
    protected event Action OnUltimateSkile;
    public override void SpecialAttack()
    {
        MageSpecialAttack();
        if (OnUltimateSkile != null) OnUltimateSkile();
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
            UsedWeapon(energyBallTransform, Get_ShootDirection(2f, target), 50);
            //CollisionWeapon UseWeapon = GetWeapon_FromPool();
            //UseWeapon.transform.position = energyBallTransform.position;
            //UseWeapon.Shoot(Get_ShootDirection(2f, target), 50, (Enemy enemy) => delegate_OnHit(enemy));
            //GameObject instantEnergyBall = CreateBullte(energyBall, energyBallTransform, delegate_OnHit);
            //ShotBullet(instantEnergyBall, 2f, 50f, target);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.NormalAttack();
    }

    void MageSpecialAttack()
    {
        isSkillAttack = true;

        MageSkile();
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
    protected void SetSkilObject(Vector3 _position)
    {
        mageEffectObject.transform.position = _position;
        mageEffectObject.SetActive(true);
    }

    public AudioClip mageSkillCilp;
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

    protected IEnumerator Play_SkillClip(AudioClip playClip, float audioSound, float audioDelay)
    {
        yield return new WaitForSeconds(audioDelay);
        if (enterStoryWorld == GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(playClip, audioSound);
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

    public virtual void MageSkile() 
    {
        isSkillAttack = true;
        ClearMana();
        StartCoroutine(Co_SkillCoolDown());
        PlaySkileAudioClip();
    }


    // 스킬 빈도 증가 이벤트
    public void SkillPercentUp()
    {
        plusMana += 20;
    }
}