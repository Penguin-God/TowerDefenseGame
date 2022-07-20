using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;

public class Multi_Unit_Mage : Multi_RangeUnit
{
    [Header("메이지 변수")]
    [SerializeField] MageUnitStat mageStat;
    protected IReadOnlyList<float> skillStats;
    [SerializeField] ProjectileData energyballData;
    [SerializeField] protected ProjectileData skillData;

    [SerializeField] GameObject magicLight;
    [SerializeField] protected Transform energyBallTransform;
    [SerializeField] protected GameObject mageSkillObject = null;

    ManaSystem manaSystem;
    public override void OnAwake()
    {
        LoadMageStat();
        SetMageAwake();

        energyballData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, energyballData.SpawnTransform);
        skillData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[1], transform, skillData.SpawnTransform);
    }

    void LoadMageStat()
    {
        if (Multi_Managers.Data.MageStatByFlag.TryGetValue(UnitFlags, out MageUnitStat stat))
        {
            mageStat = stat;
            skillStats = mageStat.SkillStats;
            manaSystem = GetComponent<ManaSystem>();
            manaSystem?.SetInfo(stat.MaxMana, stat.AddMana);
        }
    }

    // 법사 고유의 Awake 대체 가상 함수
    public virtual void SetMageAwake() { }

    bool Skillable => manaSystem != null && manaSystem.IsManaFull;
    public override void NormalAttack()
    {
        if (Skillable) SpecialAttack();
        else StartCoroutine("MageAttack");
    }

    // TODO : ManaSystem에 마나 잠그는 기능 추가한 후 삭제하기
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
            ProjectileShotDelegate.ShotProjectile(energyballData, target, OnHit);
            manaSystem?.AddMana_RPC();
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

    protected GameObject SkillSpawn(Vector3 spawnPos) => Multi_SpawnManagers.Weapon.Spawn(skillData.WeaponPath, spawnPos);
    protected virtual void MageSkile() { }

    protected void SetMageSkillStatus()
    {
        base.SpecialAttack();
        manaSystem?.ClearMana_RPC();
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