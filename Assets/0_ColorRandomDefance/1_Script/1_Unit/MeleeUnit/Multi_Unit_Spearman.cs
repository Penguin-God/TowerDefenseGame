using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Multi_Unit_Spearman : Multi_TeamSoldier
{
    [Header("창병 변수")]
    [SerializeField] Transform spearShotPoint;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    RandomExcuteSkillController _attackExcuter;
    ProjectileThrowingUnit _spearThower;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<MeeleChaser>();
        
        normalAttackSound = EffectSoundType.SpearmanAttack;
        _useSkillPercent = 30;

        _attackExcuter = gameObject.AddComponent<RandomExcuteSkillController>();
        _attackExcuter.DependencyInject(NormalAttack, SpecialAttack);

        _spearThower = gameObject.GetOrAddComponent<ProjectileThrowingUnit>();
        _spearThower.SetInfo(_throwSpearData.WeaponPath, spearShotPoint);
    }
    SpearShoter _spearShoter;
    ThrowSpearData _throwSpearData;
    public void SetSpearData(ThrowSpearDataContainer throwSpearData)
    {
        var bulider = new ResourcesPathBuilder();
        string spearPath = throwSpearData.IsMagicSpear ? bulider.BuildMagicSpaerPath(UnitColor) : bulider.BuildUnitWeaponPath(UnitFlags);
        _throwSpearData = new ThrowSpearData(spearPath, throwSpearData.RotateVector, throwSpearData.WaitForVisibilityTime, throwSpearData.AttackRate);

        if(_spearThower == null)
            _spearThower = gameObject.GetOrAddComponent<ProjectileThrowingUnit>();
        _spearThower.SetInfo(_throwSpearData.WeaponPath, spearShotPoint);
        _spearShoter = new SpearShoter(_throwSpearData, _spearThower);
    }

    protected override void AttackToAll() => _attackExcuter.RandomAttack(_useSkillPercent);

    public override void NormalAttack() => StartCoroutine(nameof(SpaerAttack));
    IEnumerator SpaerAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (PhotonNetwork.IsMasterClient)
            UnitAttacker.NormalAttack(TargetEnemy);
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        EndAttack();
    }

    public override void SpecialAttack() => StartCoroutine(nameof(Spearman_SpecialAttack));

    IEnumerator Spearman_SpecialAttack()
    {
        base.SpecialAttack();
        animator.SetTrigger("isSpecialAttack");
        if(PhotonNetwork.IsMasterClient)
            yield return StartCoroutine(_spearShoter.Co_ShotSpear(transform, spearShotPoint.position, SpearmanSkillAttack));

        spear.SetActive(false);
        nav.isStopped = true;
        PlaySound(EffectSoundType.SpearmanSkill);

        yield return new WaitForSeconds(0.5f);

        nav.isStopped = false;
        spear.SetActive(true);
        base.EndSkillAttack(_skillReboundTime);
    }

    void SpearmanSkillAttack(Multi_Enemy target) => UnitAttacker.SkillAttack(target, CalculateSpearDamage(target.enemyType));
    int CalculateSpearDamage(EnemyType enemyType) => Mathf.RoundToInt(UnitAttacker.CalculateDamage(enemyType) * _throwSpearData.AttackRate);
}

public class SpearShoter
{
    ThrowSpearData _throwSpearData;
    ProjectileThrowingUnit _spearThower;

    public SpearShoter(ThrowSpearData throwSpearData, ProjectileThrowingUnit spearThower)
    {
        _throwSpearData = throwSpearData;
        _spearThower = spearThower;
    }

    public IEnumerator Co_ShotSpear(Transform transform, Vector3 spawnPos, Action<Multi_Enemy> action)
    {
        yield return new WaitForSeconds(_throwSpearData.WaitForVisibility);
        var shotSpear = CreateThorwSpear(transform.forward, spawnPos);
        yield return new WaitForSeconds(1 - _throwSpearData.WaitForVisibility);
        ThrowSpear(shotSpear, transform.forward, action);
    }

    Multi_Projectile CreateThorwSpear(Vector3 forward, Vector3 spawnPos)
    {
        // var shotSpear = Managers.Multi.Instantiater.PhotonInstantiate(_throwSpearData.WeaponPath, spawnPos).GetComponent<Multi_Projectile>();
        // shotSpear.GetComponent<Collider>().enabled = false;
        // Quaternion lookDir = Quaternion.LookRotation(forward);
        // shotSpear.GetComponent<RPCable>().SetRotation_RPC(lookDir);

        var shotSpear = Managers.Resources.Instantiate(_throwSpearData.WeaponPath, spawnPos).GetComponent<Multi_Projectile>();
        shotSpear.GetComponent<Collider>().enabled = false;
        shotSpear.transform.rotation = Quaternion.LookRotation(forward);
        return shotSpear;
    }

    void ThrowSpear(Multi_Projectile shotSpear, Vector3 forward, Action<Multi_Enemy> action)
    {
        shotSpear.SetHitAction(action);
        shotSpear.GetComponent<Collider>().enabled = true;
        _spearThower.Throw(shotSpear, forward);
        //if (Vector3.zero != _throwSpearData.RotateVector)
        //    shotSpear.GetComponent<RPCable>().SetRotate_RPC(_throwSpearData.RotateVector);
        shotSpear.transform.Rotate(_throwSpearData.RotateVector);
    }
}
