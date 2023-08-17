using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{

}

public class MovementSlower
{
    protected readonly float SlowRate;
    protected readonly float SlowTime;

    public MovementSlower(float slowRate, float slowTime)
    {
        SlowRate = slowRate;
        SlowTime = slowTime;
    }

    public virtual void SlowToMovement(Multi_Enemy enemy) => enemy.OnSlow(SlowRate, SlowTime);
}

public class MovementSlowerUnit : MovementSlower
{
    readonly UnitFlags Flag;
    public MovementSlowerUnit(float slowRate, float slowTime, UnitFlags flag) : base(slowRate, slowTime)
        => Flag = flag;

    public override void SlowToMovement(Multi_Enemy monster)
    {
        var normalMonster = monster.GetComponent<Multi_NormalEnemy>();
        if (normalMonster != null)
            normalMonster.OnSlow(SlowRate, SlowTime, Flag);
    }
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