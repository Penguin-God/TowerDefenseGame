using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_BluePassive : Multi_UnitPassive
{
    [SerializeField] float apply_SlowPercet;
    [SerializeField] float apply_SlowTime;

    public override void SetPassive(Multi_TeamSoldier _team) => _team.OnPassiveHit += SlowByEnemy;

    void SlowByEnemy(Multi_Enemy enemy) => enemy.OnSlow(apply_SlowPercet, apply_SlowTime);

    protected override void ApplyData()
    {
        apply_SlowPercet = _stats[0];
        apply_SlowTime = _stats[1];
    }
}
