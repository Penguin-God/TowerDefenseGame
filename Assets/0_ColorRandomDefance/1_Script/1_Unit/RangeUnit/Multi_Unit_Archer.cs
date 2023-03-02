﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Multi_Unit_Archer : Multi_RangeUnit
{
    [Header("아처 변수")]
    [SerializeField] ProjectileData arrawData;
    [SerializeField] int skillArrowCount = 3;
    private GameObject trail;

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    [SerializeField] UnitRandomSkillSystem _skillSystem;

    protected override void OnAwake()
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        arrawData = new ProjectileData(new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags), transform, arrawData.SpawnTransform);
        normalAttackSound = EffectSoundType.ArcherAttack;
        _useSkillPercent = 30;
        _skillSystem = new UnitRandomSkillSystem();
    }

    [PunRPC]
    protected override void Attack() => _skillSystem.Attack(NormalAttack, SpecialAttack, _useSkillPercent);

    public override void NormalAttack() => StartCoroutine(nameof(ArrowAttack));
    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        if (PhotonNetwork.IsMasterClient && target != null && Chaseable)
        {
            //ProjectileShotDelegate.ShotProjectile(arrawData, target, OnHit);
            Show(target);
        }
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        nav.isStopped = false;
        EndAttack();
    }

    public override void SpecialAttack() => StartCoroutine(Special_ArcherAttack());
    IEnumerator Special_ArcherAttack()
    {
        base.SpecialAttack();
        trail.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            ShotSkill();

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        base.EndSkillAttack(_skillReboundTime);
    }

    void ShotSkill()
    {
        Transform[] targetArray = GetTargets();
        if (targetArray == null || targetArray.Length == 0) return;

        for (int i = 0; i < skillArrowCount; i++)
        {
            int targetIndex = i % targetArray.Length;
            //ProjectileShotDelegate.ShotProjectile(arrawData, targetArray[targetIndex], OnHit);
            Show(targetArray[targetIndex]);
        }
    }

    void Show(Transform shotTarget)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(arrawData.WeaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        photonView.RPC(nameof(Shot), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID);
        ProjectileShotDelegate.ShotProjectile(projectile, transform, shotTarget, OnHit);
    }

    [PunRPC]
    void Shot(int viewId)
    {
        var projectile = Managers.Multi.GetPhotonViewTransfrom(viewId);
        projectile.gameObject.SetActive(true);
        projectile.position = arrawData.SpawnPos;
    }

    Transform[] GetTargets()
    {
        if (TargetIsNormalEnemy == false) return new Transform[] { target };
        return Multi_EnemyManager.Instance.GetProximateEnemys(transform.position, skillArrowCount, _state.UsingId)
            .Select(x => x.transform).ToArray();
    }
}
