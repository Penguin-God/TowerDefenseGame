using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class Multi_Unit_Archer : Multi_TeamSoldier
{
    [Header("아처 변수")]
    [SerializeField] Transform arrowShotPoint;
    private GameObject trail;

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    RandomExcuteSkillController _attackExcuter;
    ArcherArrowShoter _archerArrowShoter;

    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        
        normalAttackSound = EffectSoundType.ArcherAttack;
        _attackExcuter = gameObject.AddComponent<RandomExcuteSkillController>();
        _attackExcuter.DependencyInject(NormalAttack, SpecialArcherAttack);
        _useSkillPercent = 30;
        _archerArrowShoter = new ArcherArrowShoter(TargetFinder, arrowShotPoint, GetWeaponPath());
    }

    protected override void AttackToAll() => _attackExcuter.RandomAttack(_useSkillPercent);

    protected override void NormalAttack() => StartCoroutine(nameof(ArrowAttack));
    IEnumerator ArrowAttack()
    {
        base.StartAttack();

        nav.isStopped = true;
        trail.SetActive(false);
        Managers.Resources.Instantiate(GetWeaponPath(), arrowShotPoint.position).GetComponent<Multi_Projectile>().AttackShot(GetDir(TargetEnemy), UnitAttacker.NormalAttack);
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);
        nav.isStopped = false;

        base.EndAttack();
    }

    void SpecialArcherAttack() => StartCoroutine(Special_ArcherAttack());
    IEnumerator Special_ArcherAttack()
    {
        DoAttack();

        trail.SetActive(false);
        _archerArrowShoter.ShotSkill(TargetEnemy, UnitAttacker.SkillAttack);
        yield return new WaitForSeconds(1f);
        trail.SetActive(true);

        base.EndAttack(_skillReboundTime);
    }

    Vector3 GetDir(Multi_Enemy target) => new ThorwPathCalculator().CalculateThorwPath_To_Monster(target, transform);
    string GetWeaponPath() => $"Prefabs/{new ResourcesPathBuilder().BuildUnitWeaponPath(UnitFlags)}";
}

public class ArcherArrowShoter
{
    const int ArrowCount = 3;
    readonly MonsterFinder _monsterFinder;
    Transform _shotPoint;
    readonly string Path;
    public ArcherArrowShoter(MonsterFinder monsterFinder, Transform shotPoint, string path)
    {
        _monsterFinder = monsterFinder;
        _shotPoint = shotPoint;
        Path = path;
    }

    public void ShotSkill(Multi_Enemy currentTarget, System.Action<Multi_Enemy> action)
    {
        Transform[] targetArray = GetTargets(currentTarget);
        if (targetArray == null || targetArray.Length == 0) return;

        for (int i = 0; i < ArrowCount; i++)
        {
            int targetIndex = i % targetArray.Length;
            Managers.Resources.Instantiate(Path, _shotPoint.position).GetComponent<Multi_Projectile>().AttackShot(GetDir(targetArray[targetIndex].GetComponent<Multi_Enemy>()), action);
        }
    }

    Vector3 GetDir(Multi_Enemy target) => new ThorwPathCalculator().CalculateThorwPath_To_Monster(target, _shotPoint);
    Transform[] GetTargets(Multi_Enemy currentTarget)
    {
        if (currentTarget.enemyType != EnemyType.Normal) return new Transform[] { currentTarget.transform };
        // return _monsterFinder.GetProximateEnemys(currentTarget.transform.position, ArrowCount).Select(x => x.transform).ToArray(); // currentTarget 기준으로 한 게 맞나?
        return _monsterFinder.GetProximateEnemys(_shotPoint.position, ArrowCount).Select(x => x.transform).ToArray();
    }
}