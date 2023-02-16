﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class UnitStatChangeFacade : MonoBehaviourPun
{
    MultiDataManager _unitDataManager;
    Multi_UnitManager _unitManager;
    public void Init(MultiDataManager multiDataManager, Multi_UnitManager unitManager)
    {
        _unitDataManager = multiDataManager;
        _unitManager = unitManager;
    }

    public void SetUnitStat(UnitStatType statType, int newValue) 
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, (byte)Multi_Data.instance.Id, statType, newValue);

    public void SetUnitStat(UnitStatType statType, int newValue, UnitFlags flag)
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, (byte)Multi_Data.instance.Id, statType, newValue);

    public void ScaleUnitStat(UnitStatType statType, float rate)
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, (byte)Multi_Data.instance.Id, statType, rate);

    public void ScaleUnitStat(UnitStatType statType, float rate, UnitColor unitColor)
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, (byte)Multi_Data.instance.Id, statType, rate);

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, float rate) 
        => ChangeUnitStat(id, GetUnitStatChangeAction(statType, rate));

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, int newValue)
        => ChangeUnitStat(id, GetUnitStatChangeAction(statType, newValue));

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, int newValue, UnitFlags flag)
        => ChangeUnitStat(id, GetUnitStatChangeAction(statType, newValue), x => x == flag);

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, float rate, UnitColor unitColor)
        => ChangeUnitStat(id, GetUnitStatChangeAction(statType, rate), x => x.UnitColor == unitColor);

    void ChangeUnitStat(byte id, Action<UnitStat> statChangeAction, Func<UnitFlags, bool> conditon = null)
    {
        if (conditon == null) conditon = x => true;
        ChangeUnitStatToDB(id, statChangeAction, conditon);
        if (PhotonNetwork.IsMasterClient)
            ChangeUnitStatToCurrentSpawns(id, statChangeAction, conditon);
    }

    void ChangeUnitStatToDB(byte id, Action<UnitStat> statChangeAction, Func<UnitFlags, bool> conditon)
        => _unitDataManager.ChangeUnitStat(id, statChangeAction, conditon);

    void ChangeUnitStatToCurrentSpawns(byte id, Action<UnitStat> statChangeAction, Func<UnitFlags, bool> conditon)
        => _unitManager.Master
            .GetUnits(id, unit => conditon(unit.UnitFlags))
            .ToList()
            .ForEach(x => statChangeAction(x.Stat));

    Action<UnitStat> GetUnitStatChangeAction(UnitStatType statType, int newValue)
    {
        switch (statType)
        {
            case UnitStatType.Damage:
                return x => x.SetDamage(newValue);
            case UnitStatType.BossDamage:
                return x => x.SetBossDamage(newValue);
            case UnitStatType.All:
                return x =>
                {
                    x.SetDamage(newValue);
                    x.SetBossDamage(newValue);
                };
        }
        return null;
    }

    Action<UnitStat> GetUnitStatChangeAction(UnitStatType statType, float rate)
    {
        switch (statType)
        {
            case UnitStatType.Damage:
                return x => x.SetDamage(Mathf.RoundToInt(x.Damage * rate));
            case UnitStatType.BossDamage:
                return x => x.SetBossDamage(Mathf.RoundToInt(x.BossDamage * rate));
            case UnitStatType.All:
                return x =>
                {
                    x.SetDamage(Mathf.RoundToInt(x.Damage * rate));
                    x.SetBossDamage(Mathf.RoundToInt(x.BossDamage * rate));
                };
        }
        return null;
    }
}
