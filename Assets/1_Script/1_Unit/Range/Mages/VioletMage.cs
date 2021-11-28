using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(target.position);
    }
}
