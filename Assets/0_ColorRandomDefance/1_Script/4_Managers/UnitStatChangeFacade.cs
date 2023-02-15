using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

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

    public void ChangeUnitStat(UnitStatType statType, int newValue)
        => photonView.RPC(nameof(ChangeUnitStat), RpcTarget.All, PlayerID, statType, newValue);

    [PunRPC]
    void ChangeUnitStat(byte id, UnitStatType statType, int newValue)
    {
        switch (statType)
        {
            case UnitStatType.Damage:
                ChangeUnitDatas(x => x.SetDamage(newValue));
                break;
            case UnitStatType.BossDamage:
                ChangeUnitDatas(x => x.SetBossDamage(newValue));
                break;
            case UnitStatType.All:
                ChangeUnitDatas(x => x.SetDamage(newValue));
                ChangeUnitDatas(x => x.SetBossDamage(newValue));
                break;
        }

        void ChangeUnitDatas(Action<UnitStat> action) => _unitStatData.Get(id).Values.ToList().ForEach(action);
    }
}
