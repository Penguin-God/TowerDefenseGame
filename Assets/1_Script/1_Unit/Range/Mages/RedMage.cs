using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        SetSkilObject(transform.position + (Vector3.up * 30));
        mageSkill.OnSkile(target.GetComponent<Enemy>());
    }
}
