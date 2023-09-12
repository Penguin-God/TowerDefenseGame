using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Multi_Unit_Archer : Multi_TeamSoldier
{
    [Header("아처 변수")]
    [SerializeField] ProjectileData arrawData;
    ProjectileThrowingUnit _thrower;
    [SerializeField] int skillArrowTargetCount = 3;
    private GameObject trail;

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    [SerializeField] UnitRandomSkillSystem _skillSystem;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        _thrower = gameObject.AddComponent<ProjectileThrowingUnit>();
        _thrower.SetInfo(new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags), arrawData.SpawnTransform);

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
            _thrower.FlatThrow(target, base.NormalAttack);
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

        for (int i = 0; i < skillArrowTargetCount; i++)
        {
            int targetIndex = i % targetArray.Length;
            _thrower.FlatThrow(targetArray[targetIndex], SkillAttackWithPassive);
        }
    }

    Transform[] GetTargets()
    {
        if (TargetIsNormal == false) return new Transform[] { target };
        return TargetFinder.GetProximateEnemys(transform.position, skillArrowTargetCount).Select(x => x.transform).ToArray();
    }
}
