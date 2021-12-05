using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeMage : Unit_Mage
{
    public override void MageSkile()
    {
        mageEffectObject.GetComponent<OrangeSkill>().OnSkile(target.GetComponent<Enemy>(), isUltimate, bossDamage);
        base.MageSkile();
    }
}
