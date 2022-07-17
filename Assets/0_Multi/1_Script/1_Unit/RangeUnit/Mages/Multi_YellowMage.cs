using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowMage : Multi_Unit_Mage
{
    //public override void MageSkile()
    //{
    //    if (pv.IsMine)
    //    {
    //        UsedSkill(transform.position + (Vector3.up * 0.6f));
    //        int addGold = isUltimate ? 5 : 3;
    //        Multi_GameManager.instance.AddGold(addGold);
    //    }
    //}

    protected override void _MageSkile()
    {
        SkillSpawn(transform.position + (Vector3.up * 0.6f));
        Multi_GameManager.instance.AddGold(3); // TODO : 스킬 강화 및 csv로드하면서 리터럴 값 없애기
    }
}
