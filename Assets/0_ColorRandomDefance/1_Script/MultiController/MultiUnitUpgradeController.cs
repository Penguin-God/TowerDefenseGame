using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

public class MultiUnitStatController : MonoBehaviourPun
{
    UnitStatController _unitStatCotroller;
    public void DependencyInject(UnitStatController worldUnitDamageManager) => _unitStatCotroller = worldUnitDamageManager;
    public UnitDamageInfo GetDamageInfo(UnitFlags flag) => _unitStatCotroller.GetDamageInfo(flag, Id);

    byte Id => PlayerIdManager.Id;
    public void AddUnitDamage(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        AddUnitDamageWithFlag(flag, value, changeStatType, Id);
        ClientToMaster(nameof(AddUnitDamageWithFlag), changeStatType, Id, flag, value);
    }

    public void AddUnitDamage(UnitColor color, int value, UnitStatType changeStatType)
    {
        AddUnitDamageWithColor((byte)color, value, changeStatType, Id);
        ClientToMaster(nameof(AddUnitDamageWithColor), changeStatType, Id, (byte)color, value);
    }

    public void ScaleUnitDamage(UnitColor color, float value, UnitStatType changeStatType)
    {
        ScaleUnitDamageWithColor((byte)color, value, changeStatType, Id);
        ClientToMaster(nameof(ScaleUnitDamageWithColor), changeStatType, Id, (byte)color, value);
    }

    public void ScaleAllUnitDamage(float value, UnitStatType changeStatType)
    {
        ScaleAllUnitDamage(value, changeStatType, Id);
        ClientToMaster(nameof(ScaleAllUnitDamage), changeStatType, Id, value);
    }

    [PunRPC]
    void AddUnitDamageWithFlag(UnitFlags flag, int value, UnitStatType changeStatType, byte id) => _unitStatCotroller.AddUnitDamage(flag, value, changeStatType, id);

    [PunRPC]
    void AddUnitDamageWithColor(byte color, int value, UnitStatType changeStatType, byte id) => _unitStatCotroller.AddUnitDamageWithColor((UnitColor)color, value, changeStatType, id);

    [PunRPC]
    void ScaleUnitDamageWithColor(byte color, float value, UnitStatType changeStatType, byte id) => _unitStatCotroller.ScaleUnitDamageWithColor((UnitColor)color, value, changeStatType, id);

    [PunRPC]
    void ScaleAllUnitDamage(float value, UnitStatType changeStatType, byte id) => _unitStatCotroller.ScaleAllUnitDamage(value, changeStatType, id);

    void ClientToMaster(string methodName, UnitStatType changeStatType, byte id, params object[] objects)
    {
        if (PhotonNetwork.IsMasterClient) return;
        object[] parmeters = objects.Concat(new object[] { changeStatType, id }).ToArray();
        photonView.RPC(methodName, RpcTarget.MasterClient, parmeters);
    }
}
