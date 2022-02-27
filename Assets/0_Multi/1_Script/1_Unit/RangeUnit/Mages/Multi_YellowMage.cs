using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowMage : Multi_Unit_Mage
{
    public override void MageSkile()
    {
        UsedSkill(transform.position + (Vector3.up * 0.6f));

        if (pv.IsMine)
        {
            int addGold = isUltimate ? 5 : 3;
            Multi_GameManager.instance.AddGold(addGold);
        }
    }
}
