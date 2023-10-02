using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class MultiUnitStatController : MonoBehaviourPun
{
    UnitStatController _unitStatCotroller;
    public UnitStatController UnitStatController => _unitStatCotroller;
    public void DependencyInject(UnitStatController worldUnitDamageManager) => _unitStatCotroller = worldUnitDamageManager;
    public UnitDamageInfo GetDamageInfo(UnitFlags flag) => _unitStatCotroller.GetDamageInfo(flag, Id);

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
    void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType, byte id) => _unitStatCotroller.AddUnitDamageValue(flag, value, changeStatType, id);

    [PunRPC]
    void AddUnitDamageValueWithColor(byte color, int value, UnitStatType changeStatType, byte id) => _unitStatCotroller.AddUnitDamageValueWithColor((UnitColor)color, value, changeStatType, id);

    [PunRPC]
    void ScaleUnitDamageValueWithColor(byte color, float value, UnitStatType changeStatType, byte id) => _unitStatCotroller.ScaleUnitDamageValueWithColor((UnitColor)color, value, changeStatType, id);

    [PunRPC]
    void ScaleAllUnitDamageValueWith(float value, UnitStatType changeStatType, byte id) => _unitStatCotroller.ScaleAllUnitDamageValueWith(value, changeStatType, id);
}
