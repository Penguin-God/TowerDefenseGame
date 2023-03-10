using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit
{
    readonly UnitFlags _flag;
    public Unit(UnitFlags flag) => _flag = flag;

    public UnitStat Stat;

    public event Action<Unit> OnDead;
    public void Dead()
    {
        OnDead?.Invoke(this);
        OnDead = null;
    }
}
