using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkillController
{
    public abstract void DoSkill(Unit unit, Multi_Enemy target);
    protected void PlaySkillSound(EffectSoundType type) => Managers.Sound.PlayEffect(type);
    protected GameObject SpawnSkill(SkillEffectType type, Vector3 spawnPos) => Managers.Resources.Instantiate(new ResourcesPathBuilder().BuildEffectPath(type), spawnPos);
    protected int CalculateSkillDamage(Unit unit, float rate) => Mathf.RoundToInt(Mathf.Max(unit.DamageInfo.ApplyDamage, unit.DamageInfo.ApplyBossDamage) * rate);
}

public class GainGoldController : UnitSkillController
{
    readonly int AddGold;
    readonly Transform _transform;
    readonly byte OwerId;
    readonly WorldAudioPlayer _worldAudioPlayer;
    readonly Vector3 OffSet = new Vector3(0, 0.6f, 0);
    public GainGoldController(Transform transform, int addGold, byte owerId, WorldAudioPlayer worldAudioPlayer)
    {
        AddGold = addGold;
        _transform = transform;
        OwerId = owerId;
        _worldAudioPlayer = worldAudioPlayer;
    }

    public override void DoSkill(Unit unit, Multi_Enemy target)
    {
        _worldAudioPlayer.AfterPlaySound(EffectSoundType.BlueMageSkill, 0.5f);
        SpawnSkill(SkillEffectType.YellowMagicCircle, _transform.position + OffSet);
        if(PhotonNetwork.IsMasterClient)
            Multi_GameManager.Instance.AddGold_RPC(AddGold, OwerId);
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
    public override void DoSkill(Unit unit, Multi_Enemy target)
    {
        PlaySkillSound(EffectSoundType.VioletMageSkill);
        _unit = unit;
        SpawnSkill(SkillEffectType.PosionCloud, target.transform.position + Offset).GetComponent<Multi_HitSkill>().SetHitActoin(Poison);
    }
}