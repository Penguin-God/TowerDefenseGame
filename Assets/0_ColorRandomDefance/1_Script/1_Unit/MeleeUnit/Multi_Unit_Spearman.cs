﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Multi_Unit_Spearman : Multi_TeamSoldier
{
    [Header("창병 변수")]
    [SerializeField] Transform spearShotPoint;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    SpearmanAttackController _normalAttackController;
    SpearmanSkillAttackController _skillAttackContrller;
    RandomExcuteSkillController _attackExcuter;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        
        normalAttackSound = EffectSoundType.SpearmanAttack;
        _useSkillPercent = 30;

        var attackerGenerator = new UnitAttackControllerGenerator();
        _normalAttackController = attackerGenerator.GenerateSpearmanAttcker(this);
        _skillAttackContrller = attackerGenerator.GenerateSpearmanSkillAttcker(this, _spearShoter, spearShotPoint, SpearmanSkillAttack);

        _attackExcuter = gameObject.AddComponent<RandomExcuteSkillController>();
        _attackExcuter.DependencyInject(Normal, SpecialAttack);
    }
    SpearShoter _spearShoter;
    ThrowSpearData _throwSpearData;
    public void SetSpearData(ThrowSpearDataContainer throwSpearData)
    {
        var bulider = new ResourcesPathBuilder();
        string spearPath = throwSpearData.IsMagicSpear ? bulider.BuildMagicSpaerPath(UnitColor) : bulider.BuildUnitWeaponPath(UnitFlags);
        _throwSpearData = new ThrowSpearData(spearPath, throwSpearData.RotateVector, throwSpearData.WaitForVisibilityTime, throwSpearData.AttackRate);
        _spearShoter = new SpearShoter(_throwSpearData);
    }

    void Normal() => _normalAttackController.DoAttack(AttackDelayTime);
    void SpecialAttack() => _skillAttackContrller.DoAttack(_skillReboundTime);

    protected override void AttackToAll() => _attackExcuter.RandomAttack(_useSkillPercent);

    // protected override void NormalAttack() => StartCoroutine(nameof(SpaerAttack));
    //IEnumerator SpaerAttack()
    //{
    //    base.StartAttack();

    //    animator.SetTrigger("isAttack");
    //    yield return new WaitForSeconds(0.55f);
    //    trail.SetActive(true);
    //    yield return new WaitForSeconds(0.3f);
    //    if (PhotonNetwork.IsMasterClient)
    //        UnitAttacker.NormalAttack(TargetEnemy);
    //    yield return new WaitForSeconds(0.3f);
    //    trail.SetActive(false);

    //    EndAttack();
    //}

    // void ShotSpear() => StartCoroutine(nameof(Co_ShotSpear));
    //IEnumerator Co_ShotSpear()
    //{
    //    base.DoAttack();
    //    animator.SetTrigger("isSpecialAttack");
    //    yield return StartCoroutine(_spearShoter.Co_ShotSpear(transform, spearShotPoint, SpearmanSkillAttack));

    //    spear.SetActive(false);
    //    nav.isStopped = true;
    //    PlaySound(EffectSoundType.SpearmanSkill);

    //    yield return new WaitForSeconds(0.5f);

    //    nav.isStopped = false;
    //    spear.SetActive(true);
    //    base.EndAttack(_skillReboundTime);
    //}

    void SpearmanSkillAttack(Multi_Enemy target) => UnitAttacker.SkillAttack(target, CalculateSpearDamage(target.enemyType));
    int CalculateSpearDamage(EnemyType enemyType) => Mathf.RoundToInt(UnitAttacker.CalculateDamage(enemyType) * _throwSpearData.AttackRate);
}

public class SpearShoter
{
    readonly ThrowSpearData _throwSpearData;
    public SpearShoter(ThrowSpearData throwSpearData) => _throwSpearData = throwSpearData;

    public IEnumerator Co_ShotSpear(Transform transform, Transform shotPoint, Action<Multi_Enemy> action)
    {
        yield return new WaitForSeconds(_throwSpearData.WaitForVisibility);
        var shotSpear = CreateThorwSpear(transform.forward, shotPoint.position);
        SetTrail(shotSpear, false); // 트레일 늘어지는거 방지
        yield return new WaitForSeconds(1 - _throwSpearData.WaitForVisibility);
        SetTrail(shotSpear, true);
        shotSpear.transform.position = shotPoint.position;
        ThrowSpear(shotSpear, transform.forward, action);
    }

    Multi_Projectile CreateThorwSpear(Vector3 forward, Vector3 spawnPos)
    {
        var shotSpear = Managers.Resources.Instantiate(_throwSpearData.WeaponPath, spawnPos).GetComponent<Multi_Projectile>();
        shotSpear.GetComponent<Collider>().enabled = false;
        shotSpear.transform.rotation = Quaternion.LookRotation(forward);
        return shotSpear;
    }

    void SetTrail(Component spear, bool isActive)
    {
        var trail = spear.GetComponentInChildren<TrailRenderer>();
        if(trail != null)
            trail.enabled = isActive;
    }

    void ThrowSpear(Multi_Projectile shotSpear, Vector3 forward, Action<Multi_Enemy> action)
    {
        shotSpear.GetComponent<Collider>().enabled = true;
        shotSpear.AttackShot(forward, action);
        shotSpear.transform.Rotate(_throwSpearData.RotateVector);
    }
}
