using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpecialAttackController : UnitAttackControllerTemplate
{
    [SerializeField] TrailRenderer _trail;
    ArcherArrowShoter _archerArrowShoter;
    Multi_TeamSoldier _unitController;
    UnitAttacker _attacker;

    public void Inject(ArcherArrowShoter archerArrowShoter, Multi_TeamSoldier unitController, UnitAttacker attacker)
    {
        _archerArrowShoter = archerArrowShoter;
        _unitController = unitController;
        _attacker = attacker;
    }

    protected override IEnumerator Co_Attack()
    {
        _trail.gameObject.SetActive(false);
        _archerArrowShoter.ShotSkill(_unitController.TargetEnemy, _attacker.SkillAttack);
        yield return new WaitForSeconds(1f);
        _trail.gameObject.SetActive(true);
    }
}
