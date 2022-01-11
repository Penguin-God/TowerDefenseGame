using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPassive : UnitPassive
{
    [SerializeField] float apply_UpDamageWeigh;

    [Space]
    [Space]
    [SerializeField] float upDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitDamage(teamSoldier, upDamageWeigh);
    }

    public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    {
        upDamageWeigh = p1;
        enhanced_UpDamageWeigh = en_p1;

        apply_UpDamageWeigh = upDamageWeigh;
    }

    [Space]
    [SerializeField] float enhanced_UpDamageWeigh;
    public override void Beefup_Passive()
    {
        apply_UpDamageWeigh = enhanced_UpDamageWeigh;
    }
}
