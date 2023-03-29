using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;

public class GameManagerController : MonoBehaviourPun
{
    GameManager _gamaManager;
    UnitDamageInfoManager _unitDamageManager;
    readonly UnitDamageInfoChanger _unitDamageInfoChanger = new UnitDamageInfoChanger();

    public UnitDamageInfo UnitDamageInfo(UnitFlags flag) => _unitDamageManager.GetDamageInfo(flag);
    void Awake()
    {
        _gamaManager = new GameManager(Managers.Data.Unit.DamageInfoByFlag);
        _unitDamageManager = new UnitDamageInfoManager(Managers.Data.Unit.DamageInfoByFlag);
    }

    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.AddUnitDamageValue(_unitDamageManager, flag, value, changeStatType);
        photonView.RPC(nameof(AddUnitDamageValue_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, flag, value, changeStatType);
    }

    [PunRPC]
    void AddUnitDamageValue_RPC(byte id, UnitFlags flag, int value, UnitStatType changeStatType)
    {
        _gamaManager.AddUnitDamageValue(id, flag, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, flag);
    }

    public void AddUnitDamageValue(UnitColor color, int value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.AddUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(AddUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)color, value, changeStatType);
    }

    [PunRPC]
    void AddUnitDamageValueWithColor_RPC(byte id, byte color, int value, UnitStatType changeStatType)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        _gamaManager.AddUnitDamageValue(id, conditon, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, conditon);
    }

    public void ScaleUnitDamageValue(UnitColor color, float value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)color, value, changeStatType);
    }

    [PunRPC]
    void ScaleUnitDamageValueWithColor_RPC(byte id, byte color, float value, UnitStatType changeStatType)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        _gamaManager.ScaleUnitDamageValue(id, conditon, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, conditon);
    }

    public void ScaleUnitDamageValue(float value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (x) => true, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithAll_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, value, changeStatType);
    }

    [PunRPC]
    void ScaleUnitDamageValueWithAll_RPC(byte id, float value, UnitStatType changeStatType)
    {
        _gamaManager.ScaleUnitDamageValue(id, (x) => true, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id,  (x) => true);
    }

    void UpdateCurrentUnitDamageInfo(byte id, UnitFlags targetFlag) => UpdateCurrentUnitDamageInfo(id, (flag) => flag == targetFlag); 

    void UpdateCurrentUnitDamageInfo(byte id, Func<UnitFlags, bool> condition)
        => Multi_UnitManager.Instance
            .FindUnits(x => condition(x.UnitFlags))
            .ToList()
            .ForEach(x => x.UpdateDamageInfo(_gamaManager.GetUnitDamageInfoManager(id).GetDamageInfo(x.UnitFlags)));
}
