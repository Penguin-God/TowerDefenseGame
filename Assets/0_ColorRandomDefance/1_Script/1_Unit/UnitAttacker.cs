using System;
using System.Collections;
using System.Collections.Generic;

public class UnitAttacker
{
    readonly Unit _unit;

    public UnitAttacker(Unit unit)
    {
        _unit = unit;
    }

    public void NormalAttack(Multi_Enemy target, Action<Multi_Enemy> sideEffect)
    {
        target.OnDamage(CalculrateDamage(target.enemyType), false);
        sideEffect?.Invoke(target);
    }
    int CalculrateDamage(EnemyType type) => type == EnemyType.Normal ? _unit.DamageInfo.ApplyDamage : _unit.DamageInfo.ApplyBossDamage;
}
