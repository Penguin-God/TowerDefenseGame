using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPassive : UnitPassive
{
    [SerializeField] float upDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitDamage(teamSoldier, upDamageWeigh);
    }

    public override void ApplyData(float P1, float P2 = 0, float P3 = 0)
    {
        upDamageWeigh = P1;
    }

    [Space]
    [SerializeField] float enhanced_UpDamageWeigh;
    public override void Beefup_Passive()
    {
        upDamageWeigh = enhanced_UpDamageWeigh;
    }
}
