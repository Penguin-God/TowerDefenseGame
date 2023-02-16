using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

// _unitDataManager 이용해서 데이터 베이스 값 바꾸고, 유닛 매니저 이용해서 현재 유닛 강화하는 것까지 구현하기
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

    public void ScaleUnitStat(UnitStatType statType, float rate)
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, (byte)Multi_Data.instance.Id, statType, rate);

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, float rate)
    {
        ChangeUnitStatToDB(id, GetUnitStatAction(statType, rate));
        if (PhotonNetwork.IsMasterClient)
            ChangeUnitStatToCurrentSpawns(id, GetUnitStatAction(statType, rate));
    }

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, int newValue)
    {
        ChangeUnitStatToDB(id, GetUnitStatAction(statType, newValue));
        if(PhotonNetwork.IsMasterClient)
            ChangeUnitStatToCurrentSpawns(id, GetUnitStatAction(statType, newValue));
    }

    void ChangeUnitStatToDB(byte id, Action<UnitStat> action) => _unitDataManager.ChangeAllUnitStat(id, action);
    void ChangeUnitStatToCurrentSpawns(byte id, Action<UnitStat> action) => _unitManager.Master.GetUnits(id).ToList().ForEach(x => action(x.Stat));

    Action<UnitStat> GetUnitStatAction(UnitStatType statType, int newValue)
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

    Action<UnitStat> GetUnitStatAction(UnitStatType statType, float rate)
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
