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

    protected ManaSystem manaSystem;
    protected override void OnAwake()
    {
        LoadMageStat();
        SetMageAwake();

        energyballData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, energyballData.SpawnTransform);
        skillData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[1], transform, skillData.SpawnTransform);
        normalAttackSound = EffectSoundType.MageAttack;
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

    [PunRPC]
    protected override void Attack()
    {
        if (Skillable) SpecialAttack();
        else StartCoroutine(nameof(MageAttack));
    }

    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        magicLight.SetActive(true);

        // TODO : 딱 공격하려는 순간에 적이 죽어버리면 공격을 안함. 이건 판정 문제인데 그냥 target위치를 기억해서 거기다가 던지는게 나은듯
        if (PhotonNetwork.IsMasterClient && target != null && Chaseable)
        {
            ProjectileShotDelegate.ShotProjectile(energyballData, target, OnHit);
            manaSystem?.AddMana_RPC();
        }

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.EndAttack();
    }

    [SerializeField] float mageSkillCoolDownTime;
    public override void SpecialAttack()
    {
        base.SpecialAttack();
        manaSystem?.ClearMana_RPC();
        if (PhotonNetwork.IsMasterClient)
        {
            MageSkile();
        }
        
        PlaySkillSound();
        StartCoroutine(Co_EndSkillAttack(mageSkillCoolDownTime)); // 임시방편
    }
    
    IEnumerator Co_EndSkillAttack(float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        base.EndSkillAttack(0);
    }


    protected GameObject SkillSpawn(Vector3 spawnPos) => Multi_SpawnManagers.Weapon.Spawn(skillData.WeaponPath, spawnPos);
    protected virtual void MageSkile() { }

    protected virtual void PlaySkillSound() { }
}