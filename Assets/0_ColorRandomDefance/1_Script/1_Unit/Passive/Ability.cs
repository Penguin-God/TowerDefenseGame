using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSlower
{
    protected readonly float SlowRate;
    protected readonly float SlowTime;

    public MonsterSlower(float slowRate, float slowTime)
    {
        SlowRate = slowRate;
        SlowTime = slowTime;
    }

    public void SlowToMovement(Multi_Enemy monster)
    {
        var normalMonster = monster.GetComponent<Multi_NormalEnemy>();
        if (normalMonster != null)
            ApplySlow(normalMonster);
    }

    public void SlowToMovement(Multi_Enemy monster, float rate)
    {
        var normalMonster = monster.GetComponent<Multi_NormalEnemy>();
        if (normalMonster != null)
            ApplySlow(normalMonster);
    }

    public virtual void SlowToMovementWhitTime(Multi_Enemy monster)
    {
        var normalMonster = monster.GetComponent<Multi_NormalEnemy>();
        if (normalMonster != null)
            ApplySlow(normalMonster);
    }

    protected virtual void ApplySlow(Multi_NormalEnemy monster) => monster.OnSlowWithTime(SlowRate, SlowTime);
}

public class MovementSlowerUnit : MonsterSlower
{
    readonly UnitFlags Flag;
    public MovementSlowerUnit(float slowRate, float slowTime, UnitFlags flag) : base(slowRate, slowTime)
        => Flag = flag;

    protected override void ApplySlow(Multi_NormalEnemy monster) => monster.OnSlowWithTime(SlowRate, SlowTime, Flag);
}
