using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class MagicFountainController : UnitSkillController
{
    readonly int AttackCount;
    readonly float HpRate;
    readonly WorldAudioPlayer _audioPlayer;

    public MagicFountainController(int attackCount, float damageRate, WorldAudioPlayer audioPlayer)
    {
        AttackCount = attackCount;
        HpRate = damageRate;
        _audioPlayer = audioPlayer;
    }

    public override void DoSkill(Multi_TeamSoldier unit)
        => SpawnSkill(SkillEffectType.OrangeWater, unit.target.position).GetComponent<Multi_OrangeSkill>().OnSkile(unit.target.GetComponent<Multi_Enemy>(), unit.BossDamage, AttackCount, HpRate, _audioPlayer);
}

public class MultiVectorShotController : UnitSkillController
{
    readonly float DamageRate;
    const int ShotCount = 8;
    readonly Vector3 Offset = new Vector3(0, 2, 0);
    public MultiVectorShotController(float damageRate) => DamageRate = damageRate;

    public override void DoSkill(Multi_TeamSoldier unit)
    {
        PlaySkillSound(unit, EffectSoundType.BlackMageSkill);
        foreach (Vector3 dir in MathUtil.CalculateDirections(ShotCount).Select(x => new Vector3(x.x, 0, x.y)))
            SpawnSkill(SkillEffectType.BlackEnergyBall, unit.transform.position + Offset).GetComponent<Multi_Projectile>().AttackShot(dir.normalized, AttackMonster);

        void AttackMonster(Multi_Enemy enemy) => enemy.OnDamage(CalculateSkillDamage(unit.Unit, DamageRate), isSkill: true);
    }
}

public class IceCloudController : UnitSkillController
{
    readonly float FreezeTime;
    readonly Vector3 Offset = new Vector3(0, 2, 0);
    public IceCloudController(float freezeTime) => FreezeTime = freezeTime;

    public override void DoSkill(Multi_TeamSoldier unit)
    {
        PlaySkillSound(unit, EffectSoundType.BlueMageSkill);
        SpawnSkill(SkillEffectType.IceCloud, unit.transform.position + Offset).GetComponent<Multi_HitSkill>().SetHitActoin(FreezeMonster);

        void FreezeMonster(Multi_Enemy monster) => monster.GetComponent<Multi_NormalEnemy>()?.OnFreeze(FreezeTime, unit.UnitFlags);
    }
}

public class MeteorShotController : UnitSkillController
{
    readonly float DamRate;
    readonly float StunTime;
    readonly MeteorController _meteorController;
    readonly Vector3 Offset = new Vector3(0, 30, 3);

    public MeteorShotController(float damRate, float stunTime, MeteorController meteorController)
    {
        DamRate = damRate;
        StunTime = stunTime;
        _meteorController = meteorController;
    }

    public override void DoSkill(Multi_TeamSoldier unit)
    {
        if(PhotonNetwork.IsMasterClient)
            _meteorController.ShotMeteor(unit.target.GetComponent<Multi_Enemy>(), CalculateSkillDamage(unit.Unit, DamRate), StunTime, unit.transform.position + Offset);
    }
}