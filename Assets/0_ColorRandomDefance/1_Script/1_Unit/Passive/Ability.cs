using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{

}

public class MovementSlower
{
    readonly float SlowRate;
    readonly float SlowTime;

    public MovementSlower(float slowRate, float slowTime)
    {
        SlowRate = slowRate;
        SlowTime = slowTime;
    }

    public void SlowToMovement(Multi_Enemy enemy) => enemy.OnSlow(SlowRate, SlowTime);
}

public class StunAbility
{
    readonly int SturnPercent;
    readonly float StrunTime;

    public StunAbility(int sturnPercent, float strunTime)
    {
        SturnPercent = sturnPercent;
        StrunTime = strunTime;
    }

    public void StunToMovement(Multi_Enemy _enemy) => _enemy.OnStun_RPC(SturnPercent, StrunTime);
}

public class PoisonAbility
{
    readonly int PoisonTickCount;
    readonly int MaxPoisonDamage;

    public PoisonAbility(int poisonTickCount, int maxPoisonDamage)
    {
        PoisonTickCount = poisonTickCount;
        MaxPoisonDamage = maxPoisonDamage;
    }

    public void PosionToMonster(Multi_Enemy _enemy) => _enemy.OnPoison_RPC(PoisonTickCount, MaxPoisonDamage);
}