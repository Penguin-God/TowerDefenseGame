using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_YellowMage : Multi_Unit_Mage
{
    [SerializeField] int addGold;

    public override void SetMageAwake()
    {
        addGold = (int)skillStats[0];
    }

    protected override void PlaySkillSound() => AfterPlaySound(EffectSoundType.BlackMageSkill, 0.5f);
    protected override void MageSkile()
    {
        SkillSpawn(transform.position + (Vector3.up * 0.6f));
        Multi_GameManager.instance.AddGold_RPC(addGold, rpcable.UsingId);
    }
}
