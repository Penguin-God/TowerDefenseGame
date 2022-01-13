using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPassive : UnitPassive
{
    [SerializeField] float apply_UpDamageWeigh;

    //[Space]
    //[Space]
    //[SerializeField] float upDamageWeigh;

    public override void SetPassive()
    {
        EventManager.instance.ChangeUnitDamage(teamSoldier, apply_UpDamageWeigh);
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_UpDamageWeigh = p1;
    }

    //[Space]
    //[SerializeField] float enhanced_UpDamageWeigh;
    //public override void Beefup_Passive()
    //{
    //    apply_UpDamageWeigh = enhanced_UpDamageWeigh;
    //}
}
