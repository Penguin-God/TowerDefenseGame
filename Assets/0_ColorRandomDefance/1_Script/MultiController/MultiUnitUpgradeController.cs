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
        AddUnitDamageValueWithColor((byte)color, value, changeStatType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(AddUnitDamageValueWithColor), RpcTarget.MasterClient, (byte)color, value, changeStatType, Id);
    }

    public void ScaleUnitDamageValue(UnitColor color, float value, UnitStatType changeStatType)
    {
        ScaleUnitDamageValueWithColor((byte)color, value, changeStatType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(ScaleUnitDamageValueWithColor), RpcTarget.MasterClient, (byte)color, value, changeStatType, Id);
    }

    public void ScaleUnitDamageValue(float value, UnitStatType changeStatType)
    {
        ScaleAllUnitDamageValueWith(value, changeStatType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(ScaleAllUnitDamageValueWith), RpcTarget.MasterClient, value, changeStatType, Id);
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
