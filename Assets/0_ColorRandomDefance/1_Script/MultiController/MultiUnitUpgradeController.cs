using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;

public class MultiUnitDamageManagerController : MonoBehaviourPun
{
    WorldUnitDamageManager worldUnitDamageManager;
    byte Id => PlayerIdManager.Id;
    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        AddUnitDamageValue(flag, value, changeStatType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(AddUnitDamageValue), RpcTarget.MasterClient, flag, value, changeStatType, Id);
    }

    public void AddUnitDamageValue(UnitColor color, int value, UnitStatType changeStatType)
    {
        // _unitDamageInfoChanger.AddUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(AddUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, Id, (byte)color, value, changeStatType);
    }

    public void ScaleUnitDamageValue(UnitColor color, float value, UnitStatType changeStatType)
    {
        // _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, Id, (byte)color, value, changeStatType);
    }

    public void ScaleUnitDamageValue(float value, UnitStatType changeStatType)
    {
        // _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (x) => true, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithAll_RPC), RpcTarget.MasterClient, Id, value, changeStatType);
    }

    [PunRPC]
    void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        worldUnitDamageManager.AddUnitDamageValue(flag, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, flag);
    }

    [PunRPC]
    void AddUnitDamageValueWithColor(byte color, int value, UnitStatType changeStatType, byte id)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        worldUnitDamageManager.AddUnitDamageValue(conditon, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, conditon);
    }

    [PunRPC]
    void ScaleUnitDamageValueWithColor(byte color, float value, UnitStatType changeStatType, byte id)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        worldUnitDamageManager.ScaleUnitDamageValue(conditon, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, conditon);
    }

    [PunRPC]
    void ScaleAllUnitDamageValueWith(float value, UnitStatType changeStatType, byte id)
    {
        worldUnitDamageManager.ScaleUnitDamageValue((x) => true, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, (x) => true);
    }
}
