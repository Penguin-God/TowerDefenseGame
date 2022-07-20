using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_RedPassive : Multi_UnitPassive
{
    [SerializeField] float apply_DownDelayWeigh = 0;

    // 법사를 위한 비율 관측용
    public float Get_DownDelayWeigh { get { return apply_DownDelayWeigh; } }

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        _team.AttackDelayTime *= apply_DownDelayWeigh;
    }

    protected override void ApplyData()
    {
        apply_DownDelayWeigh = _stats[0];
    }
}
