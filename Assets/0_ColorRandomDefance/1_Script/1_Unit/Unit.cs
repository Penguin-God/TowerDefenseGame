using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit
{
    private readonly UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    readonly UnitDamageInfo _damageInfo;
    public UnitDamageInfo DamageIndo => _damageInfo;

    public Action<Multi_Enemy> OnHit;
    public Action<Multi_Enemy> OnPassiveHit;
    public event Action<Unit> OnDead;

    public Unit(UnitFlags flag, int damage, int bossDamage)
    {
        _unitFlags = flag;
        _damageInfo = new UnitDamageInfo(damage, bossDamage);
    }

    public Unit(UnitFlags flag) => new Unit(flag, 0, 0);

    public void Dead() => OnDead?.Invoke(this);
}
