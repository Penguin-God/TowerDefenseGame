using UnityEngine;
using System.Linq;

public class Multi_Unit_Archer : Multi_TeamSoldier
{
    [Header("아처 변수")]
    [SerializeField] Transform arrowShotPoint;

    [SerializeField] int _useSkillPercent;
    [SerializeField] float _skillReboundTime;
    RandomExcuteSkillController _attackExcuter;
    ArcherNormalAttackController _normalAttackController;
    ArcherSpecialAttackController _specialAttackController;
    protected override void OnAwake()
    {
        _chaseSystem = gameObject.AddComponent<RangeChaser>();
        
        var attackerGenerator = new UnitAttackControllerGenerator();
        _normalAttackController = attackerGenerator.GenerateArcherAttacker(this, arrowShotPoint);
        _specialAttackController = attackerGenerator.GenerateArcherSkillAttcker(this, new ArcherArrowShoter(TargetFinder, arrowShotPoint, GetWeaponPath()));
        _attackExcuter = gameObject.AddComponent<RandomExcuteSkillController>();
        _attackExcuter.DependencyInject(Normal, SpecialAttack);
        _useSkillPercent = 30;
    }

    void Normal() => _normalAttackController.DoAttack(AttackDelayTime);
    void SpecialAttack() => _specialAttackController.DoAttack(_skillReboundTime);

    protected override void AttackToAll()
    {
        _attackExcuter.RandomAttack(_useSkillPercent);
        print("이건 화살이라고 합니다");
    }
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
        if(currentTarget == null) return;
        Transform[] targetArray = GetTargets(currentTarget);
        if (targetArray == null || targetArray.Length == 0) return;

        for (int i = 0; i < ArrowCount; i++)
        {
            int targetIndex = i % targetArray.Length;
            Managers.Resources.Instantiate(Path, _shotPoint.position).GetComponent<UnitProjectile>().AttackShot(GetDir(targetArray[targetIndex]), action);
            Debug.Log($"연발 화살 방향 : {GetDir(targetArray[targetIndex])}");
        }
    }

    Vector3 GetDir(Transform target) => new ThorwPathCalculator().CalculateThorwPath_To_Monster(target.GetComponent<Multi_Enemy>(), _shotPoint);
    Transform[] GetTargets(Multi_Enemy currentTarget)
    {
        if (currentTarget.enemyType != EnemyType.Normal) return new Transform[] { currentTarget.transform };
        return _monsterFinder.GetProximateEnemys(currentTarget.transform.position, ArrowCount).Select(x => x.transform).ToArray(); // currentTarget 기준으로 한 게 맞나?
        // return _monsterFinder.GetProximateEnemys(_shotPoint.position, ArrowCount).Select(x => x.transform).ToArray();
    }
}