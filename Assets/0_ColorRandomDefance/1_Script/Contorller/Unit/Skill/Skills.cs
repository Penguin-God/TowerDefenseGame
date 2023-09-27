using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkillController
{
    public abstract void DoSkill(Multi_TeamSoldier unit);
    protected void PlaySkillSound(Multi_TeamSoldier unit, EffectSoundType type, float delay = 0) => unit.AfterPlaySound(type, delay);
    protected GameObject SpawnSkill(SkillEffectType type, Vector3 spawnPos) => Managers.Resources.Instantiate(new ResourcesPathBuilder().BuildEffectPath(type), spawnPos);
    protected int CalculateSkillDamage(Unit unit, float rate) => Mathf.RoundToInt(Mathf.Max(unit.DamageInfo.ApplyDamage, unit.DamageInfo.ApplyBossDamage) * rate);
}

public class GainGoldController : UnitSkillController
{
    readonly int AddGold;
    readonly Vector3 OffSet = new Vector3(0, 0.6f, 0);
    public GainGoldController(int addGold) => AddGold = addGold;

    public override void DoSkill(Multi_TeamSoldier unit)
    {
        PlaySkillSound(unit, EffectSoundType.BlueMageSkill, delay: 0.5f);
        SpawnSkill(SkillEffectType.YellowMagicCircle, unit.transform.position + OffSet);
        if(PhotonNetwork.IsMasterClient)
            Multi_GameManager.Instance.AddGold_RPC(AddGold, unit.Spot.WorldId);
    }
}

public class PoisonCloudController : UnitSkillController
{
    readonly int PoisonCount;
    readonly float DamageRate;
    Vector3 Offset = new Vector3(0, 2, 0);

    public PoisonCloudController(int poisonCount, float damageRate)
    {
        PoisonCount = poisonCount;
        DamageRate = damageRate;
    }

    Unit _unit;
    void Poison(Multi_Enemy target) => target.OnPoison_RPC(PoisonCount, CalculateSkillDamage(_unit, DamageRate), true);
    public override void DoSkill(Multi_TeamSoldier unit)
    {
        PlaySkillSound(unit, EffectSoundType.VioletMageSkill);
        _unit = unit.Unit;
        SpawnSkill(SkillEffectType.PosionCloud, unit.target.position + Offset).GetComponent<Multi_HitSkill>().SetHitActoin(Poison);
    }
}