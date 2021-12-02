using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePassive : UnitPassive
{
    [SerializeField] float slowPercent;
    [SerializeField] float slowTime;

    public override void SetPassive()
    {
        teamSoldier.delegate_OnPassive += (Enemy enemy) => enemy.EnemySlow(slowPercent, slowTime);
    }
}
