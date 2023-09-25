using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkillController
{
    public abstract void DoSkill();
    protected void PlaySkillSound(EffectSoundType type) => Managers.Sound.PlayEffect(type);
}

public class GainGoldController : UnitSkillController
{
    readonly int AddGold;
    public GainGoldController(Unit unit, int addGold) => AddGold = addGold;

    public override void DoSkill()
    {
        // SkillSpawn(transform.position + (Vector3.up * 0.6f));
        // Multi_GameManager.Instance.AddGold_RPC(AddGold, rpcable.UsingId);
    }
}