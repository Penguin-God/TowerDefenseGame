using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherNormalAttackController : UnitAttackControllerTemplate
{
    [SerializeField] TrailRenderer _trail;
    NavMeshAgent _nav;
    Transform _arrowShotPoint;
    Multi_TeamSoldier _unitController;
    UnitAttacker _attacker;
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
    }

    public void Inject(Transform arrowShotPoint, Multi_TeamSoldier unitController, UnitAttacker attacker)
    {
        _arrowShotPoint = arrowShotPoint;
        _unitController = unitController;
        _attacker = attacker;
    }

    protected override IEnumerator Co_Attack()
    {
        _nav.isStopped = true;
        _trail.gameObject.SetActive(false);
        Managers.Resources.Instantiate(ArrowPath, _arrowShotPoint.position).GetComponent<Multi_Projectile>().AttackShot(GetDir(), _attacker.NormalAttack);
        yield return WaitSecond(1f);
        _trail.gameObject.SetActive(true);
        _nav.isStopped = false;
    }
    Vector3 GetDir() => new ThorwPathCalculator().CalculateThorwPath_To_Monster(_unitController.TargetEnemy, transform);
    string ArrowPath => new ResourcesPathBuilder().BuildUnitWeaponPath(_unit.UnitFlags);
}
