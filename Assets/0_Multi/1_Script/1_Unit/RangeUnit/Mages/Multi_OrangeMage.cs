using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangeMage : Multi_Unit_Mage
{
    public override void MageSkile()
    {
        if (pv.IsMine)
        {
            GameObject _skill = UsedSkill(Vector3.one);
            _skill.GetComponent<Multi_OrangeSkill>().OnSkile(target.GetComponentInParent<Multi_Enemy>(), isUltimate, bossDamage);
        } 
    }
}