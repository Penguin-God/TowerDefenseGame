using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAttackPassive
{
    void DoUnitPassive(Multi_Enemy target);
}

public class MonsterSlower : IUnitAttackPassive
{
    readonly int SlowRate;
    readonly int SlowTime;
    readonly UnitFlags Flag;

    public MonsterSlower(int slowRate, int slowTime, UnitFlags flag)
    {
        SlowRate = slowRate;
        SlowTime = slowTime;
        Flag = flag;
    }

    public void DoUnitPassive(Multi_Enemy target) => target.GetComponent<Multi_NormalEnemy>()?.OnSlowWithTime(SlowRate, SlowTime, Flag);
}

public class GoldenAttacker : IUnitAttackPassive
{
    readonly int GoldGainRate;
    readonly int GainGold;
    readonly byte OwnerId;
    public GoldenAttacker(int goldGainRate, int gainGold, byte ownerId)
    {
        GoldGainRate = goldGainRate;
        GainGold = gainGold;
        OwnerId = ownerId;
    }

    public void DoUnitPassive(Multi_Enemy target)
    {
        int random = Random.Range(0, 100);
        if (random < GoldGainRate)
        {
            Multi_GameManager.Instance.AddGold_RPC(GainGold, OwnerId);
            Managers.Sound.PlayEffect(EffectSoundType.GetPassiveGold);
        }
    }
}

public class PosionAndStunActor : IUnitAttackPassive
{
    readonly int SturnPercent;
    readonly float StrunTime;
    readonly int PoisonTickCount;
    readonly float PosionDamageRate;
    readonly Unit _unit;

    public PosionAndStunActor(int sturnPercent, float strunTime, int poisonTickCount, float posionDamageRate, Unit unit)
    {
        SturnPercent = sturnPercent;
        StrunTime = strunTime;
        PoisonTickCount = poisonTickCount;
        PosionDamageRate = posionDamageRate;
        _unit = unit;
    }

    public void DoUnitPassive(Multi_Enemy target)
    {
        target.GetComponent<Multi_NormalEnemy>()?.OnStun_RPC(SturnPercent, StrunTime);
        target.OnPoison_RPC(PoisonTickCount, CalculatePosionDamage(target.enemyType), isSkill: true);
    }

    int CalculatePosionDamage(EnemyType enemyType)
    {
        if (EnemyType.Normal == enemyType) return Mathf.RoundToInt(_unit.DamageInfo.ApplyDamage * PosionDamageRate);
        else return Mathf.RoundToInt(_unit.DamageInfo.ApplyBossDamage * PosionDamageRate);
    }
}