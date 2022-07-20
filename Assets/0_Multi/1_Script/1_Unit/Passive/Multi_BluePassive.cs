using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_BluePassive : Multi_UnitPassive
{
    [SerializeField] float apply_SlowPercet;
    [SerializeField] float apply_SlowTime;

    // 법사가 쓰기 위한 변수들
    public float Get_SlowPercent => apply_SlowPercet;
    public float Get_ColliderRange => apply_SlowTime; // 법사 패시브에서 slowTime은 무한이므로 콜라이더 범위 변수로 씀

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        _team.OnPassiveHit += SlowByEnemy;
    }

    void SlowByEnemy(Multi_Enemy enemy) => enemy.OnSlow_RPC(apply_SlowPercet, apply_SlowTime);

    protected override void ApplyData()
    {
        apply_SlowPercet = _stats[0];
        apply_SlowTime = _stats[1];
    }
}
