using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 2));
    }
}
