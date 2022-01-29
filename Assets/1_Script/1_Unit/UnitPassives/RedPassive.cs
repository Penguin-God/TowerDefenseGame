using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPassive : UnitPassive
{
    [SerializeField] float apply_DownDelayWeigh = 0;

    // 법사를 위한 비율 관측용
    public float Get_DownDelayWeigh { get { return apply_DownDelayWeigh; } }

    public override void SetPassive(TeamSoldier _team)
    {
        _team.attackDelayTime *= apply_DownDelayWeigh;
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_DownDelayWeigh = p1;
    }
}
