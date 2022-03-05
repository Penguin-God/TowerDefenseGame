using System.Collections.Generic;
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
        poolManager.SettingWeaponPool(energyBall, 7);
        if (unitColor == UnitColor.white) return;

        canvasRectTransform = transform.parent.GetComponentInChildren<RectTransform>();
        manaSlider = transform.parent.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        StartCoroutine(Co_SetCanvas());

        SetMageAwake();
    }

    public override void NormalAttack() => StartCoroutine("MageAttack");

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
            poolManager.UsedWeapon(energyBallTransform, Get_ShootDirection(2f, target), 50, (Enemy enemy) => delegate_OnHit(enemy));
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.NormalAttack();
    }


    Queue<GameObject> skillObjectPool = new Queue<GameObject>();
    
    protected void SettingSkilePool(GameObject skillObj, int count, Action<GameObject> SettingSkileAction = null)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject skill = Instantiate(skillObj, new Vector3(-200, -200, -200), skillObj.transform.rotation);
            // Hit Event 설정해줘야 하는 법사들만 실행됨
            if (SettingSkileAction != null) SettingSkileAction(skill);

            skill.SetActive(false);
            skillObjectPool.Enqueue(skill);
        }
    }

    protected void UpdatePool(Action<GameObject> updateSkill)
    {
        if(updateSkill != null)
        {
            for (int i = 0; i < skillObjectPool.Count; i++)
            {
                GameObject _skill = skillObjectPool.Dequeue();
                updateSkill(_skill);
                skillObjectPool.Enqueue(_skill);
            }
        }
    }

    [SerializeField] protected GameObject mageSkillObject = null;
    protected GameObject UsedSkill(Vector3 _position)
    {
        GameObject _skileObj = GetSkile_FromPool();
        _skileObj.transform.position = _position;
        return _skileObj;
    }

    GameObject GetSkile_FromPool()
    {
        GameObject getSkile = skillObjectPool.Dequeue();
        getSkile.SetActive(true);
        StartCoroutine(Co_ReturnSkile_ToPool(getSkile, 5f));
        return getSkile;
    }
    IEnumerator Co_ReturnSkile_ToPool(GameObject _skill, float time)
    {
        yield return new WaitForSeconds(time);
        _skill.SetActive(false);
        _skill.transform.position = new Vector3(-200, -200, -200);
        skillObjectPool.Enqueue(_skill);
    }

    public virtual void SetMageAwake()
    {
        SettingSkilePool(mageSkillObject, 3);
    }

    public virtual void MageSkile()
    {
        isSkillAttack = true;
        ClearMana();
        StartCoroutine(Co_SkillCoolDown());
        PlaySkileAudioClip();
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

    public bool isUltimate; // 스킬 강화
    protected event Action OnUltimateSkile; // 강화는 isUltimate가 true될 때까지 코루틴에서 WaitUntil로 대기 후 추가함
    public override void SpecialAttack()
    {
        MageSpecialAttack();
        if (OnUltimateSkile != null) OnUltimateSkile();
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


    // 스킬 빈도 증가 이벤트
    public void SkillPercentUp()
    {
        plusMana += 20;
    }
}