using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        GameObject _skill = UsedSkill(Vector3.one);
        _skill.GetComponent<OrangeSkill>().OnSkile(target.GetComponent<Enemy>(), isUltimate, bossDamage);
    }
}
