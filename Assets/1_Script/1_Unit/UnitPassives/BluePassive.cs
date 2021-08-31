using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePassive : UnitPassive
{
    [SerializeField] float slowPercent;
    [SerializeField] float slowTime;

    public override void SetPassive()
    {
        base.SetPassive();
        teamSoldier.delegate_OnHit += (Enemy enemy) => EnemySlow(enemy);
    }

    void EnemySlow(Enemy enemy)
    {
        enemy.EnemySlow(slowPercent, slowTime);
    }
}
