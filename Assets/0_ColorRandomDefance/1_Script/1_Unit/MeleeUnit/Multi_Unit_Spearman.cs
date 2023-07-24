using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Spearman : Multi_MeleeUnit
{
    [Header("창병 변수")]

    [SerializeField] ProjectileData shotSpearData;
    ProjectileThrowingUnit _spearThower;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    [SerializeField] UnitRandomSkillSystem _skillSystem;

    protected override void OnAwake()
    {
        _spearThower = gameObject.AddComponent<ProjectileThrowingUnit>();
        _spearThower.SetInfo(_throwSpearData.WeaponPath, shotSpearData.SpawnTransform);
        
        normalAttackSound = EffectSoundType.SpearmanAttack;
        _useSkillPercent = 30;
        _skillSystem = new UnitRandomSkillSystem();
    }

    ThrowSpearData _throwSpearData;
    public void SetSpearData(ThrowSpearDataContainer throwSpearData)
    {
        var bulider = new ResourcesPathBuilder();
        string spearPath = throwSpearData.IsMagic ? bulider.BuildMagicSpaerPath(UnitColor) : bulider.BuildUnitWeaponPath(UnitFlags);
        _throwSpearData = new ThrowSpearData(spearPath, throwSpearData.RotateVector, throwSpearData.WaitForVisibilityTime, throwSpearData.AttackRate);
    }

    [PunRPC]
    protected override void Attack() => _skillSystem.Attack(NormalAttack, SpecialAttack, _useSkillPercent);
    public override void NormalAttack() => StartCoroutine(nameof(SpaerAttack));
    IEnumerator SpaerAttack()
    {
        base.StartAttack();

        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(0.55f);
        trail.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (PhotonNetwork.IsMasterClient)
        {
            HitMeeleAttack();
        }
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
            StartCoroutine(Co_ShotSpear());

        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;
        PlaySound(EffectSoundType.SpearmanSkill);

        yield return new WaitForSeconds(0.5f);

        nav.isStopped = false;
        spear.SetActive(true);
        base.EndSkillAttack(_skillReboundTime);
    }

    IEnumerator Co_ShotSpear()
    {
        yield return new WaitForSeconds(_throwSpearData.WaitForVisibility);
        var shotSpear = CreateThorwSpear();
        yield return new WaitForSeconds(1 - _throwSpearData.WaitForVisibility);
        ThrowSpear(shotSpear);
    }

    Multi_Projectile CreateThorwSpear()
    {
        var shotSpear = Managers.Multi.Instantiater.PhotonInstantiate(_throwSpearData.WeaponPath, shotSpearData.SpawnTransform.position).GetComponent<Multi_Projectile>();
        shotSpear.GetComponent<Collider>().enabled = false;
        Quaternion lookDir = Quaternion.LookRotation(transform.forward);
        shotSpear.GetComponent<RPCable>().SetRotation_RPC(lookDir);
        return shotSpear;
    }

    void ThrowSpear(Multi_Projectile shotSpear)
    {
        shotSpear.SetHitAction(monster => SkillAttackWithPassive(monster, Mathf.RoundToInt(CalaulateAttack() * _throwSpearData.AttackRate)));
        shotSpear.GetComponent<Collider>().enabled = true;
        _spearThower.Throw(shotSpear, transform.forward);
        if (Vector3.zero != _throwSpearData.RotateVector)
            shotSpear.GetComponent<RPCable>().SetRotate_RPC(_throwSpearData.RotateVector);
    }
}
