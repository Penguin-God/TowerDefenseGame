using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_OrangePassive : Multi_UnitPassive
{
    [SerializeField] float apply_UpBossDamageWeigh;

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        _team.BossDamage += Mathf.FloorToInt(_team.OriginBossDamage * apply_UpBossDamageWeigh);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpBossDamageWeigh = p1;
    }

    protected override void ApplyData()
    {
        apply_UpBossDamageWeigh = _stats[0];
    }
}
