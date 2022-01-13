using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPassive : UnitPassive
{
    [SerializeField] float apply_DownDelayWeigh = 0;

    //[Space]
    //[Space]
    //[SerializeField] float downDelayWeigh = 0;

    // 법사를 위한 비율 관측용
    public float Get_DownDelayWeigh { get { return apply_DownDelayWeigh; } }

    public override void SetPassive()
    {
        teamSoldier.attackDelayTime *= apply_DownDelayWeigh;
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_DownDelayWeigh = p1;
    }

    //[Space]
    //[SerializeField] float enhanced_DownDelayWeigh = 0;
    //public override void Beefup_Passive()
    //{
    //    apply_DownDelayWeigh = enhanced_DownDelayWeigh;
    //}

    //public override void ApplyData(float p1, float en_p1, float p2 = 0, float en_p2 = 0, float p3 = 0, float en_p3 = 0)
    //{
    //    downDelayWeigh = p1;
    //    enhanced_DownDelayWeigh = en_p1;

    //    apply_DownDelayWeigh = downDelayWeigh;
    //}
}
