using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPassive : UnitPassive
{
    [SerializeField] float upDamageWeigh;

    public override void SetPassive()
    {
        base.SetPassive();
        EventManager.instance.ChangeUnitDamage(teamSoldier, upDamageWeigh);
    }
}
