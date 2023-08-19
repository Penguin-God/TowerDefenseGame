using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BluePassive : Multi_UnitPassive
{
    Multi_TeamSoldier _team;
    [SerializeField] float slowTime;
    public override void SetPassive(Multi_TeamSoldier team)
    {
        team.OnPassiveHit += SlowByEnemy;
        _team = team;
    }

    void SlowByEnemy(Multi_Enemy enemy) => enemy.GetComponent<Multi_NormalEnemy>()?.OnSlowWithTime(_stats[0], _stats[1], _team.UnitFlags);

    protected override void ApplyData()
    {
        slowTime = _stats[1];
    }
}
