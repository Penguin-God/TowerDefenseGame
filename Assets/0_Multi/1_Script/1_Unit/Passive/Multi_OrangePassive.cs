﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangePassive : Multi_UnitPassive
{
    [SerializeField] float apply_UpBossDamageWeigh;

    public override void SetPassive(TeamSoldier _team)
    {
        EventManager.instance.ChangeUnitBossDamage(_team, apply_UpBossDamageWeigh);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpBossDamageWeigh = p1;
    }
}
