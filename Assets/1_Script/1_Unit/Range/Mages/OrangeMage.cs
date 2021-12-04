using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeMage : Unit_Mage
{
    MageSkill mageSkile;

    public override void SetMageAwake()
    {
        base.SetMageAwake();
        mageSkile = mageEffectObject.GetComponent<MageSkill>();
    }

    public override void MageSkile()
    {
        mageSkile.GetComponent<OrangeSkill>().team = this;
        mageSkile.OnSkile(target.GetComponent<Enemy>());
        base.MageSkile();
    }
}
