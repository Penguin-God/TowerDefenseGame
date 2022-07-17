using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;

public class SkillWeapon
{
    string _weaponName;

    public SkillWeapon(string weaponName)
    {
        _weaponName = weaponName;
    }

    public GameObject Spawn(Vector3 pos) => Multi_SpawnManagers.Weapon.Spawn(WeaponType.MageSkills, _weaponName, pos);
}

public class Multi_Unit_Mage : Multi_RangeUnit
{
    [Header("메이지 변수")]
    [SerializeField] ProjectileData energyballData;
    [SerializeField] protected ProjectileData skillData;

    [SerializeField] GameObject magicLight;
    [SerializeField] protected Transform energyBallTransform;
    [SerializeField] protected GameObject mageSkillObject = null;

    [SerializeField] string skillWeaponName;
    SkillWeapon skillWeapon;

    public override void OnAwake()
    {
        if (unitColor == UnitColor.white) return;

        canvasRectTransform = transform.GetComponentInChildren<RectTransform>();
        manaSlider = transform.GetComponentInChildren<Slider>();
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        StartCoroutine(Co_SetCanvas());

        SetMageAwake();
        skillWeapon = new SkillWeapon(skillWeaponName);
        Debug.Assert(energyballData.Original != null && energyballData.SpawnTransform != null, "energyballData가 설정되어 있지 않음");
        energyballData = 
            new ProjectileData(energyballData.Original, Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, energyballData.SpawnTransform);
        skillData =
            new ProjectileData(null, Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[1], transform, null);

        //arrawData = new ProjectileData(arrawData.Original, Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, arrawData.SpawnTransform);
        //energyballData = new ProjectileData(energyballData.Original, transform, energyballData.SpawnTransform);
    }

    // 법사 고유의 Awake 대체 가상 함수
    public virtual void SetMageAwake() { }

    // 지금은 테스트를 위해 첫 공격에 무조건 스킬 쓰도록 놔둠
    private bool isMageSkillAttack;
    public override void NormalAttack()
    {
        if (true) SpecialAttack();
        else StartCoroutine("MageAttack");
    }

    [SerializeField] int plusMana = 30;
    public int PlusMana { get { return plusMana; } set { plusMana = value; }  }
    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        magicLight.SetActive(true);

        // TODO : 딱 공격하려는 순간에 적이 죽어버리면 공격을 안함. 이건 판정 문제인데 그냥 target위치를 기억해서 거기다가 던지는게 나은듯
        if (PhotonNetwork.IsMasterClient && target != null && enemyDistance < chaseRange)
        {
            ProjectileShotDelegate.ShotProjectile(energyballData, target, 2, OnHit);
            pv.RPC("AddMana", RpcTarget.All, plusMana);
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        EndAttack();
    }

    [SerializeField] float mageSkillCoolDownTime;
    public bool isUltimate; // 스킬 강화
    protected event Action OnUltimateSkile; // 강화는 isUltimate가 true될 때까지 코루틴에서 WaitUntil로 대기 후 추가함
    public override void SpecialAttack()
    {
        SetMageSkillStatus();
        if (PhotonNetwork.IsMasterClient)
        {
            MageSkile();
        }
        PlaySkileAudioClip();
        SkillCoolDown(mageSkillCoolDownTime);
        if (OnUltimateSkile != null) OnUltimateSkile();
    }

    protected GameObject SkillSpawn(Vector3 spawnPos) => skillWeapon.Spawn(spawnPos);
    protected virtual void MageSkile() { }

    protected void SetMageSkillStatus()
    {
        base.SpecialAttack();
        ClearMana();
    }

    // 마나
    [SerializeField] private int maxMana;
    [SerializeField] private int currentMana;
    
    [PunRPC]
    void AddMana(int addMana)
    {
        if (unitColor == UnitColor.white) return;

        currentMana += addMana;
        manaSlider.value = currentMana;
        if (currentMana >= maxMana) isMageSkillAttack = true;
    }

    void ClearMana()
    {
        currentMana = 0;
        manaSlider.value = 0;
        isMageSkillAttack = false;
    }

    private RectTransform canvasRectTransform;
    private Slider manaSlider;
    Vector3 sliderDir = new Vector3(90, 0, 0);
    IEnumerator Co_SetCanvas()
    {
        if (unitColor == UnitColor.white) yield break;
        while (true)
        {
            canvasRectTransform.rotation = Quaternion.Euler(sliderDir);
            yield return null;
        }
    }


    // 사운드
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
}