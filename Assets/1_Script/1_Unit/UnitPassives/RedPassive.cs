using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPassive : UnitPassive
{
    [SerializeField] float downDelayWeigh = 0;

    // 법사를 위한 비율 관측용
    public float get_DownDelayWeigh { get { return downDelayWeigh; } }

    public override void SetPassive()
    {
        teamSoldier.attackDelayTime *= downDelayWeigh;
    }

    [Space]
    [SerializeField] float enhanced_DownDelayWeigh = 0;
    public override void Beefup_Passive()
    {
        downDelayWeigh = enhanced_DownDelayWeigh;
    }
}
