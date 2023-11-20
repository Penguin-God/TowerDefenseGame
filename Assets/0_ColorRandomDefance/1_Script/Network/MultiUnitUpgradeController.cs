using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

public enum UnitStatUpgradeType
{
    Values,
    Dam,
    BossDam,
    Rates,
    BossDamRate,
}

public class MultiUnitStatController : MonoBehaviourPun
{
    UnitStatController _unitStatCotroller;
    public void DependencyInject(UnitStatController worldUnitDamageManager) => _unitStatCotroller = worldUnitDamageManager;
    public UnitDamageInfo GetDamageInfo(UnitFlags flag) => _unitStatCotroller.GetDamageInfo(flag, Id);

    byte Id => PlayerIdManager.Id;

    public void UpgradeUnitDamage(UnitFlags flag, int value, UnitStatUpgradeType upgradeType) // 여기서 DamInfo 받기
    {
        UpgradeUnitDamage(flag, value, upgradeType, Id);
        if(PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(UpgradeUnitDamage), RpcTarget.MasterClient, flag, value, upgradeType, Id);
    }

    public void UpgradeUnitDamage(UnitColor color, int value, UnitStatUpgradeType upgradeType)
    {
        UpgradeUnitDamage(color, value, upgradeType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(UpgradeUnitDamage), RpcTarget.MasterClient, color, value, upgradeType, Id);
    }

    public void UpgradeUnitDamage(int value, UnitStatUpgradeType upgradeType)
    {
        UpgradeUnitDamage(value, upgradeType, Id);
        if (PhotonNetwork.IsMasterClient == false)
            photonView.RPC(nameof(UpgradeUnitDamage), RpcTarget.MasterClient, value, upgradeType, Id);
    }

    //public void AddUnitDamage(UnitFlags flag, int value, UnitStatType changeStatType)
    //{
    //    AddUnitDamageWithFlag(flag, value, changeStatType, Id);
    //    ClientToMaster(nameof(AddUnitDamageWithFlag), changeStatType, Id, flag, value);
    //}

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

    [PunRPC] void UpgradeUnitDamage(UnitFlags flag, int value, UnitStatUpgradeType type, byte id) => UpgradeUnitDamage(x => x == flag, value, type, id);
    [PunRPC] void UpgradeUnitDamage(UnitColor color, int value, UnitStatUpgradeType type, byte id) => UpgradeUnitDamage(x => x.UnitColor == color, value, type, id);
    [PunRPC] void UpgradeUnitDamage(int value, UnitStatUpgradeType type, byte id) => UpgradeUnitDamage(x => true, value, type, id);

    void UpgradeUnitDamage(Func<UnitFlags, bool> conditoin, int value, UnitStatUpgradeType type, byte id) => _unitStatCotroller.UpgradeUnitDamage(conditoin, CraeteUpgardeInfo(value, type), id);
    UnitDamageInfo CraeteUpgardeInfo(int value, UnitStatUpgradeType type)
    {
        switch (type)
        {
            case UnitStatUpgradeType.Values: return new UnitDamageInfo(dam: value, bossDam: value, damRate: 0);
            case UnitStatUpgradeType.Dam: return new UnitDamageInfo(dam: value);
            case UnitStatUpgradeType.BossDam: return new UnitDamageInfo(bossDam: value);
            case UnitStatUpgradeType.Rates: return new UnitDamageInfo(damRate: value / 100f, bossDamRate: value / 100f);
            case UnitStatUpgradeType.BossDamRate: return new UnitDamageInfo(bossDamRate: value / 100f);
            default: return new UnitDamageInfo();
        }
    }

    //[PunRPC]
    //void AddUnitDamageWithFlag(UnitFlags flag, int value, UnitStatType changeStatType, byte id) => _unitStatCotroller.AddUnitDamage(flag, value, changeStatType, id);

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
