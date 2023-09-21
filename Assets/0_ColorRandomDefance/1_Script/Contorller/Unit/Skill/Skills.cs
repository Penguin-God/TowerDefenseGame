using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSkill
{
    void DoSkill();
}

public class GainGold : IUnitSkill
{
    readonly int AddGold;
    public GainGold(int addGold) => AddGold = addGold;

    // protected override void PlaySkillSound() => AfterPlaySound(EffectSoundType.BlackMageSkill, 0.5f);
    public void DoSkill()
    {
        // SkillSpawn(transform.position + (Vector3.up * 0.6f));
        // Multi_GameManager.Instance.AddGold_RPC(AddGold, rpcable.UsingId);
    }
}