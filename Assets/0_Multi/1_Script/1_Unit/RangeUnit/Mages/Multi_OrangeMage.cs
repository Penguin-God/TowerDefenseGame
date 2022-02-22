using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangeMage : Multi_Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        GameObject _skill = skillPoolManager.UsedSkill(Vector3.one);
        _skill.GetComponent<OrangeSkill>().OnSkile(target.GetComponent<Enemy>(), isUltimate, bossDamage);
    }
}
