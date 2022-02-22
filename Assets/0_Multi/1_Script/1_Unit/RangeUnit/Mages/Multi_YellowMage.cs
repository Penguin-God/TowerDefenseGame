using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowMage : Multi_Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        skillPoolManager.UsedSkill(transform.position + (Vector3.up * 0.6f));

        int addGold = isUltimate ? 5 : 3;
        Multi_GameManager.instance.AddGold(addGold);
    }
}
