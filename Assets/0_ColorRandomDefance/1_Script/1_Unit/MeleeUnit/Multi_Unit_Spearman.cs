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
    public void InjectSpearData(ThrowSpearData throwSpearData) => _throwSpearData = throwSpearData;

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
    IEnumerator _Spearman_SpecialAttack()
    {
        base.SpecialAttack();
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;

        if (PhotonNetwork.IsMasterClient && target != null)
        {
            Multi_Projectile weapon = _spearThower.Throw(transform.forward, SkillAttackWithPassive);
            weapon.GetComponent<RPCable>().SetRotate_RPC(new Vector3(90, 0, 0));
        }

        PlaySound(EffectSoundType.SpearmanSkill);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        base.EndSkillAttack(_skillReboundTime);
    }

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
        var shotSpear = _spearThower.CreateProjectile(_throwSpearData.WeaponPath, SkillAttackWithPassive);
        yield return new WaitForSeconds(1 - _throwSpearData.WaitForVisibility);

        _spearThower.Throw(shotSpear, transform.forward);
        shotSpear.GetComponent<RPCable>().SetRotate_RPC(_throwSpearData.RotateVector);
    }
}
