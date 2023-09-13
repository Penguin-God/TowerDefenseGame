using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAttackPassive
{
    void DoUnitPassive(Unit unit, Multi_Enemy target);
}

public class MonsterSlower : IUnitAttackPassive
{
    readonly int SlowRate;
    readonly int SlowTime;

    public MonsterSlower(int slowRate, int slowTime)
    {
        SlowRate = slowRate;
        SlowTime = slowTime;
    }

    public void DoUnitPassive(Unit unit, Multi_Enemy target) => target.GetComponent<Multi_NormalEnemy>()?.OnSlowWithTime(SlowRate, SlowTime, unit.UnitFlags);
}

public class GoldenAttacker : IUnitAttackPassive
{
    readonly int GoldGainRate;
    readonly int GainGold;

    public GoldenAttacker(int goldGainRate, int gainGold)
    {
        GoldGainRate = goldGainRate;
        GainGold = gainGold;
    }

    public void DoUnitPassive(Unit unit, Multi_Enemy target)
    {
        int random = Random.Range(0, 100);
        if (random < GoldGainRate)
        {
            Multi_GameManager.Instance.AddGold_RPC(GainGold, unit.UnitSpot.WorldId);
            Managers.Sound.PlayEffect(EffectSoundType.GetPassiveGold);
        }
    }
}

public class PosionAndStunActor : IUnitAttackPassive
{
    readonly int SturnPercent;
    readonly float StrunTime;
    readonly int PoisonTickCount;
    readonly int MaxPoisonDamage;

    public PosionAndStunActor(int sturnPercent, float strunTime, int poisonTickCount, int maxPoisonDamage)
    {
        SturnPercent = sturnPercent;
        StrunTime = strunTime;
        PoisonTickCount = poisonTickCount;
        MaxPoisonDamage = maxPoisonDamage;
    }

    public void DoUnitPassive(Unit unit, Multi_Enemy target)
    {
        target.OnStun_RPC(SturnPercent, StrunTime);
        target.OnPoison_RPC(PoisonTickCount, MaxPoisonDamage, isSkill: true);
    }
}