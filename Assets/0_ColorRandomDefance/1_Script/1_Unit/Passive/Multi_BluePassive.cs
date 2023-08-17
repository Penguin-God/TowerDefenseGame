using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BluePassive : Multi_UnitPassive
{
    MovementSlowerUnit _slower;
    public override void SetPassive(Multi_TeamSoldier team)
    {
        team.OnPassiveHit += SlowByEnemy;
        _slower = new MovementSlowerUnit(_stats[0], _stats[1], team.UnitFlags);
    }

    void SlowByEnemy(Multi_Enemy enemy) => _slower.SlowToMovement(enemy);
}
