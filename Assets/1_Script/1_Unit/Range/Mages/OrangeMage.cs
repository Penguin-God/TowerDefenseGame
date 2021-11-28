using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeMage : Unit_Mage
{
    public override void MageSkile()
    {
        mageSkill.GetComponent<OrangeSkill>().team = this;
        mageSkill.OnSkile(target.GetComponent<Enemy>());
        base.MageSkile();
    }
}
