using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPassive : UnitPassive
{
    [SerializeField] float downDelayWeigh;
    public override void SetPassive()
    {
        teamSoldier.attackDelayTime *= downDelayWeigh;
    }
}
