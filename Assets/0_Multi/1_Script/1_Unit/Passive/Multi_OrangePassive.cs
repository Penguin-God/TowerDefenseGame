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

    protected override void ApplyData()
    {
        apply_UpBossDamageWeigh = _stats[0];
    }
}
