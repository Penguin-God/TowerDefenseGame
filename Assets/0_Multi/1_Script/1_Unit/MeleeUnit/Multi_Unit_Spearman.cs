using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_Unit_Spearman : Multi_MeleeUnit
{
    [Header("창병 변수")]

    [SerializeField] ProjectileData shotSpearData;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject spear; // 평타칠 때 쓰는 창

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    [SerializeField] UnitRandomSkillSystem _skillSystem;

    protected override void OnAwake()
    {
        shotSpearData = new ProjectileData(Multi_Managers.Data.WeaponDataByUnitFlag[UnitFlags].Paths[0], transform, shotSpearData.SpawnTransform);
        normalAttackSound = EffectSoundType.SpearmanAttack;
        _useSkillPercent = 30;
        _skillSystem = new UnitRandomSkillSystem(this, 1.5f);
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
        if(pv.IsMine) HitMeeleAttack();
        yield return new WaitForSeconds(0.3f);
        trail.SetActive(false);

        EndAttack();
    }

    void OnSkillHit(Multi_Enemy enemy) => base.SkillAttackToEnemy(enemy, _skillSystem.GetApplyDamage(enemy));

    public override void SpecialAttack() => StartCoroutine(nameof(Spearman_SpecialAttack));
    IEnumerator Spearman_SpecialAttack()
    {
        base.SpecialAttack();
        animator.SetTrigger("isSpecialAttack");
        yield return new WaitForSeconds(1f);

        spear.SetActive(false);
        nav.isStopped = true;

        if (PhotonNetwork.IsMasterClient)
        {
            Multi_Projectile weapon = ProjectileShotDelegate.ShotProjectile(shotSpearData, transform.forward, OnSkillHit);
            weapon.GetComponent<RPCable>().SetRotate_RPC(new Vector3(90, 0, 0));
        }

        PlaySound(EffectSoundType.SpearmanSkill);

        yield return new WaitForSeconds(0.5f);
        nav.isStopped = false;
        spear.SetActive(true);

        base.EndSkillAttack(_skillReboundTime);
    }
}
