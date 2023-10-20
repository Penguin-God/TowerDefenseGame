using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MageSpellController : UnitAttackControllerTemplate
{
    protected override void Awake()
    {
        base.Awake();
        _nav = GetComponent<NavMeshAgent>();
    }

    public void DependencyInject(ManaSystem manaSystem, UnitSkillController unitSkillController, float skillCastingTime)
    {
        _manaSystem = manaSystem;
        _unitSkillController = unitSkillController;
        _skillCastingTime = skillCastingTime;
    }

    NavMeshAgent _nav;
    ManaSystem _manaSystem;
    UnitSkillController _unitSkillController;
    float _skillCastingTime;
    protected override IEnumerator Co_Attack()
    {
        _nav.isStopped = true;
        _manaSystem.ClearMana();
        _unitSkillController.DoSkill(GetComponent<Multi_TeamSoldier>());
        yield return new WaitForSeconds(_skillCastingTime);
        _nav.isStopped = false;
    }
}
