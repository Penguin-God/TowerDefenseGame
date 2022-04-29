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
    // 법사 패시브에서 slowTime은 무한이므로 콜라이더 범위 변수로 씀
    public float Get_ColliderRange => apply_SlowTime;
    public event Action OnBeefup = null;

    public override void SetPassive(Multi_TeamSoldier _team)
    {
        apply_SlowPercet = 30;
        apply_SlowTime = 2;
        //_team.delegate_OnPassive += (Multi_Enemy enemy) => enemy.photonView.RPC("OnSlow", RpcTarget.MasterClient, apply_SlowPercet, apply_SlowTime);
        _team.OnPassive += OnPassive;
    }

    public override void ApplyData(float p1, float p2 = 0, float p3 = 0)
    {
        apply_SlowPercet = p1;
        apply_SlowTime = p2;
    }

    void OnPassive(Multi_Enemy enemy) => enemy.OnSlow(RpcTarget.MasterClient, apply_SlowPercet, apply_SlowTime);
}
