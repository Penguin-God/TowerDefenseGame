using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_VioletMage : Multi_Unit_Mage
{
    // TODO : 리터럴 죽이기
    protected override void MageSkile()
    {
        SkillSpawn(target.position + Vector3.up * 2).GetComponent<Multi_HitSkill>().OnHitSkile += (enemy) => enemy.OnPoison_RPC(25, 8, 0.3f, 120000);
    }
}
