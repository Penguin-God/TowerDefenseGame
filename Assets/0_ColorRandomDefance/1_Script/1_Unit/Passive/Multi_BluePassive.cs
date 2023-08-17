using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BluePassive : Multi_UnitPassive
{
    [SerializeField] float apply_SlowPercet;
    [SerializeField] float apply_SlowTime;
    MovementSlowerUnit _slower;
    public override void SetPassive(Multi_TeamSoldier team)
    {
        team.OnPassiveHit += SlowByEnemy;
        _slower = new MovementSlowerUnit(_stats[0], _stats[1], team.UnitFlags);
    }

    void SlowByEnemy(Multi_Enemy enemy) => _slower.SlowToMovement(enemy); // enemy.OnSlow(apply_SlowPercet, apply_SlowTime);

    protected override void ApplyData()
    {
        apply_SlowPercet = _stats[0];
        apply_SlowTime = _stats[1];
    }
}
