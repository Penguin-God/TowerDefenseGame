using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;

public class SkillObjectPoolManager : MonoBehaviourPun
{
    Queue<GameObject> skillObjectPool = new Queue<GameObject>();

    public void SettingSkilePool(GameObject skillObj, int count, Action<GameObject> SettingSkileAction = null)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject skill = PhotonNetwork.Instantiate(skillObj.name, new Vector3(-200, -200, -200), skillObj.transform.rotation);
            // Hit Event 설정해줘야 하는 법사들만 실행됨
            if (SettingSkileAction != null) SettingSkileAction(skill);

            skill.SetActive(false);
            skillObjectPool.Enqueue(skill);
        }
    }

    public GameObject UsedSkill(Vector3 _position)
    {
        GameObject _skileObj = GetSkile_FromPool();
        _skileObj.GetComponent<MyPunRPC>().RPC_Position(_position);
        return _skileObj;
    }

    GameObject GetSkile_FromPool()
    {
        GameObject getSkile = skillObjectPool.Dequeue();
        getSkile.GetComponent<MyPunRPC>().RPC_Active(true);
        StartCoroutine(Co_ReturnSkile_ToPool(getSkile, 5f));
        return getSkile;
    }

    IEnumerator Co_ReturnSkile_ToPool(GameObject _skill, float time)
    {
        yield return new WaitForSeconds(time);
        _skill.GetComponent<MyPunRPC>().RPC_Active(false);
        _skill.GetComponent<MyPunRPC>().RPC_Position(new Vector3(200, 200, 200));
        skillObjectPool.Enqueue(_skill);
    }

    // 스킬 강화용
    public void UpdatePool(Action<GameObject> updateSkill)
    {
        if (updateSkill != null)
        {
            for (int i = 0; i < skillObjectPool.Count; i++)
            {
                GameObject _skill = skillObjectPool.Dequeue();
                updateSkill(_skill);
                skillObjectPool.Enqueue(_skill);
            }
        }
    }
}

public class Multi_Unit_Mage : Multi_RangeUnit
{
    [Header("메이지 변수")]
    [SerializeField] GameObject magicLight;

    [SerializeField] GameObject energyBall;
    [SerializeField] Transform energyBallTransform;
    [SerializeField] protected GameObject mageSkillObject = null;

    protected SkillObjectPoolManager skillPoolManager = null;
    public override void OnAwake()
    {
        poolManager.SettingWeaponPool(energyBall, 7);
        if (unitColor == UnitColor.white) return;

        canvasRectTransform = transform.GetComponentInChildren<RectTransform>();
        manaSlider = transform.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        StartCoroutine(Co_SetCanvas());

        gameObject.AddComponent<SkillObjectPoolManager>();
        skillPoolManager = GetComponent<SkillObjectPoolManager>();
        SetMageAwake();
    }

    // 법사 고유의 스킬 오브젝트 세팅 가상 함수
    public virtual void SetMageAwake() => skillPoolManager.SettingSkilePool(mageSkillObject, 3);

    protected GameObject UsedSkill(Vector3 _pos) => skillPoolManager.UsedSkill(_pos);
    protected void UpdateSkill(Action<GameObject> _act) => skillPoolManager.UpdatePool(_act);


    // 지금은 테스트를 위해 첫 공격이 무조건 스킬 쓰도록 놔둠
    private bool isMageSkillAttack = true;
    public override void NormalAttack()
    {
        if (!isMageSkillAttack) StartCoroutine("MageAttack");
        else SpecialAttack();
    }

    [SerializeField] int plusMana = 30;
    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);

        AddMana(plusMana);
        magicLight.SetActive(true);
        if (target != null && enemyDistance < chaseRange && pv.IsMine)
        {
            poolManager.UsedWeapon(energyBallTransform, Get_ShootDirection(2f, target), 50, (Multi_Enemy enemy) => delegate_OnHit(enemy));
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        EndAttack();
    }


    public bool isUltimate; // 스킬 강화
    protected event Action OnUltimateSkile; // 강화는 isUltimate가 true될 때까지 코루틴에서 WaitUntil로 대기 후 추가함
    public override void SpecialAttack()
    {
        isAttack = false;
        isAttackDelayTime = false;
        MageSpecialAttack();
        if (OnUltimateSkile != null) OnUltimateSkile();
    }

    void MageSpecialAttack()
    {
        MageSkile();
        SetMageSkillStatus();
    }

    // 법사 스킬
    public virtual void MageSkile() {}

    protected void SetMageSkillStatus()
    {
        isSkillAttack = true;
        ClearMana();
        StartCoroutine(Co_SkillCoolDown());
        PlaySkileAudioClip();
    }

    [SerializeField] float mageSkillCoolDownTime;
    IEnumerator Co_SkillCoolDown()
    {
        yield return new WaitForSeconds(mageSkillCoolDownTime);
        isSkillAttack = false;
    }

    [SerializeField] private int maxMana;
    [SerializeField] private int currentMana;
    public void AddMana(int addMana)
    {
        if (unitColor == UnitColor.white) return;
        currentMana += addMana;
        manaSlider.value = currentMana;
        if (currentMana >= maxMana) isMageSkillAttack = true;
    }

    public void ClearMana()
    {
        currentMana = 0;
        manaSlider.value = 0;
        isMageSkillAttack = false;
    }


    [SerializeField] AudioClip mageSkillCilp;
    protected void PlaySkileAudioClip()
    {
        switch (unitColor)
        {
            case UnitColor.red: StartCoroutine(Play_SkillClip(mageSkillCilp, 1f, 0f)); break;
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
        if (enterStoryWorld == Multi_GameManager.instance.playerEnterStoryMode)
            unitAudioSource.PlayOneShot(playClip, audioSound);
    }


    private RectTransform canvasRectTransform;
    private Slider manaSlider;
    IEnumerator Co_SetCanvas()
    {
        if (unitColor == UnitColor.white) yield break;
        while (true)
        {
            canvasRectTransform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            yield return null;
        }
    }
}