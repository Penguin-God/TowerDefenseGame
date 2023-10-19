using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkillAttackController : UnitAttackControllerTemplate
{
    ManaSystem _manaSystem;
    UnitSkillController _unitSkillController;
    float _mageSkillCoolDownTime;
    protected override IEnumerator Co_Attack()
    {
        _manaSystem.ClearMana_RPC();
        _unitSkillController.DoSkill(GetComponent<Multi_TeamSoldier>());
        yield return new WaitForSeconds(_mageSkillCoolDownTime);
    }
}
