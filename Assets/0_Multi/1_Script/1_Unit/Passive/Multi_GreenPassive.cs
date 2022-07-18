using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_GreenPassive : Multi_UnitPassive
{
    [SerializeField] float apply_UpDamageWeigh;

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        _team.Damage += Mathf.FloorToInt(apply_UpDamageWeigh * _team.OriginDamage);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpDamageWeigh = p1;
    }

    protected override void ApplyData()
    {
        apply_UpDamageWeigh = _stats[0];
    }
}
