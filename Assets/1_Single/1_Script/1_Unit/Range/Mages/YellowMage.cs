using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowMage : Unit_Mage
{
    public override void MageSkile()
    {
        base.MageSkile();
        UsedSkill(transform.position + (Vector3.up * 0.6f));

        int addGold = isUltimate ? 5 : 3;
        GameManager.instance.Gold += addGold;
        UIManager.instance.UpdateGoldText(GameManager.instance.Gold);
    }
}
