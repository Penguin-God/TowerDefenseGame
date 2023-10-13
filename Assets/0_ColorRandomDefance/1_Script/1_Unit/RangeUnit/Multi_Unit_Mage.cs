﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Unit_Mage : Multi_TeamSoldier
{
    [Header("메이지 변수")]
    [SerializeField] MageUnitStat mageStat;
    protected IReadOnlyList<float> skillStats;
    
    [SerializeField] GameObject magicLight;
    [SerializeField] Transform energyBallShotPoint;

    protected ManaSystem manaSystem;
    MageAttackerController _normalAttacker;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        LoadMageStat();
        SetMageAwake();

        normalAttackSound = EffectSoundType.MageAttack;
        _normalAttacker = new UnitAttackControllerGenerator().GenerateMageAattacker(this, manaSystem, ShotEnergyBall);
    }

    void LoadMageStat()
    {
        if (Managers.Data.MageStatByFlag.TryGetValue(UnitFlags, out MageUnitStat stat))
        {
            mageStat = stat;
            skillStats = mageStat.SkillStats;
            manaSystem = GetComponent<ManaSystem>();
            manaSystem?.SetInfo(stat.MaxMana, stat.AddMana);
        }
    }

    // 법사 고유의 Awake 대체 가상 함수
    public virtual void SetMageAwake() { }
    public void InjectSkillController(UnitSkillController unitSkillController) => _unitSkillController = unitSkillController;
    bool Skillable => manaSystem != null && manaSystem.IsManaFull;

    [PunRPC]
    protected override void Attack()
    {
        if (Skillable) MageSkile();
        // else StartCoroutine(nameof(MageAttack));
        else _normalAttacker.DoAttack(1, AttackDelayTime);
    }

    protected IEnumerator MageAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.7f);
        magicLight.SetActive(true);

        ShotEnergyBall(GetWeaponPath(), UnitAttacker.NormalAttack);
        if (PhotonNetwork.IsMasterClient)
            manaSystem?.AddMana_RPC();

        yield return new WaitForSeconds(0.5f);
        magicLight.SetActive(false);
        nav.isStopped = false;

        base.EndAttack();
    }

    void ShotEnergyBall(Vector3 pos) => ShotEnergyBall(GetWeaponPath(), UnitAttacker.NormalAttack, pos);
    protected void ShotEnergyBall(string path, Action<Multi_Enemy> hit, Vector3 shotPos) => Managers.Resources.Instantiate(path, shotPos).GetComponent<Multi_Projectile>().AttackShot(GetDir(), hit);
    protected void ShotEnergyBall(string path, Action<Multi_Enemy> hit) => Managers.Resources.Instantiate(path, energyBallShotPoint.position).GetComponent<Multi_Projectile>().AttackShot(GetDir(), hit);
    string GetWeaponPath() => new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags);
    Vector3 GetDir() => new ThorwPathCalculator().CalculateThorwPath_To_Monster(TargetEnemy, transform);

    [SerializeField] float mageSkillCoolDownTime;
    protected UnitSkillController _unitSkillController = null;
    void MageSkile()
    {
        DoAttack();
        manaSystem?.ClearMana_RPC();
        if (_unitSkillController != null && PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(DoSkill), RpcTarget.All, target.GetComponent<PhotonView>().ViewID);
        else
            _MageSkile();

        PlaySkillSound();
        StartCoroutine(Co_EndSkillAttack(mageSkillCoolDownTime)); // 임시방편
    }
    
    IEnumerator Co_EndSkillAttack(float skillTime)
    {
        yield return new WaitForSeconds(skillTime);
        base.EndAttack(0);
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        manaSystem.ClearMana_RPC();
    }

    [PunRPC] 
    void DoSkill(int targetId)
    {
        _targetManager.ChangedTarget(Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(targetId));
        _unitSkillController.DoSkill(this);
    }
    protected int CalculateSkillDamage(float rate) => Mathf.RoundToInt(Mathf.Max(Damage, BossDamage) * rate);
    protected virtual void _MageSkile() { }
    protected virtual void PlaySkillSound() { }
}