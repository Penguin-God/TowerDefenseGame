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
    private GameObject trail;

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    [SerializeField] UnitRandomSkillSystem _skillSystem;

    ArcherArrowShoter _archerArrowShoter;

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
        _archerArrowShoter = new ArcherArrowShoter(TargetFinder, _thrower);
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

        if (PhotonNetwork.IsMasterClient && target != null)
            _archerArrowShoter.ShotSkill(TargetEnemy, SkillAttackWithPassive);

        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        base.EndSkillAttack(_skillReboundTime);
    }
}

public class ArcherArrowShoter
{
    const int ArrowCount = 3;
    readonly MonsterFinder _monsterFinder;
    ProjectileThrowingUnit _thrower;
    public ArcherArrowShoter(MonsterFinder monsterFinder, ProjectileThrowingUnit thrower)
    {
        _monsterFinder = monsterFinder;
        _thrower = thrower;
    }

    public void ShotSkill(Multi_Enemy currentTarget, System.Action<Multi_Enemy> action)
    {
        Transform[] targetArray = GetTargets(currentTarget);
        if (targetArray == null || targetArray.Length == 0) return;

        for (int i = 0; i < ArrowCount; i++)
        {
            int targetIndex = i % targetArray.Length;
            _thrower.FlatThrow(targetArray[targetIndex], action);
        }
    }

    Transform[] GetTargets(Multi_Enemy currentTarget)
    {
        if (currentTarget.enemyType != EnemyType.Normal) return new Transform[] { currentTarget.transform };
        return _monsterFinder.GetProximateEnemys(currentTarget.transform.position, ArrowCount).Select(x => x.transform).ToArray();
    }
}