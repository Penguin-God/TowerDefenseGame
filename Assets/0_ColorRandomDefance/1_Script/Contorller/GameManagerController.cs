using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManagerController : MonoBehaviourPun
{
    GameManager _gamaManager;
    UnitDamageInfoManager _unitDamageManager;
    public UnitDamageInfo UnitDamageInfo(UnitFlags flag) => _unitDamageManager.GetDamageInfo(flag);
    void Awake()
    {
        _gamaManager = new GameManager(Managers.Data.Unit.DamageInfoByFlag);
        _unitDamageManager = new UnitDamageInfoManager(Managers.Data.Unit.DamageInfoByFlag);
    }

    public void ChangeUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        new UnitDamageInfoChanger().ChangeUnitDamageValue(_unitDamageManager, flag, value, changeStatType);
        photonView.RPC(nameof(ChangeUnitDamageValue_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, flag, value, changeStatType);
    }
    [PunRPC]
    void ChangeUnitDamageValue_RPC(byte id, UnitFlags flag, int value, UnitStatType changeStatType)
    {
        _gamaManager.ChangeUnitDamageValue(id, flag, value, changeStatType);
        UpdateChangeUnitDamageInfo(id, flag);
    }

    public void ScaleUnitDamageValue(UnitFlags flag, float value, UnitStatType changeStatType)
    {
        new UnitDamageInfoChanger().ScaleUnitDamageValue(_unitDamageManager, flag, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValue_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, flag, value, changeStatType);
    }
    [PunRPC]
    void ScaleUnitDamageValue_RPC(byte id, UnitFlags flag, float value, UnitStatType changeStatType)
    {
        _gamaManager.ScaleUnitDamageValue(id, flag, value, changeStatType);
        UpdateChangeUnitDamageInfo(id, flag);
    }

    void UpdateChangeUnitDamageInfo(byte id, UnitFlags flag) 
        => Multi_UnitManager.Instance
            .FindUnits(x => x.UnitFlags == flag)
            .ToList()
            .ForEach(x => x.UpdateDamageInfo(_gamaManager.GetUnitDamageInfoManager(id).GetDamageInfo(flag)));
}
