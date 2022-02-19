using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_GreenPassive : Multi_UnitPassive
{
    [SerializeField] float apply_UpDamageWeigh;

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        //EventManager.instance.ChangeUnitDamage(_team, apply_UpDamageWeigh);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpDamageWeigh = p1;
    }
}
