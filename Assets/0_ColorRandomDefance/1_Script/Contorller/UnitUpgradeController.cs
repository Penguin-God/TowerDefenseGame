using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}

public class UnitUpgradeController : MonoBehaviourPun
{
    UnitDamageInfoManager _unitDamageManager;
    readonly UnitDamageInfoChanger _unitDamageInfoChanger = new UnitDamageInfoChanger();

    public UnitDamageInfo GetUnitDamageInfo(UnitFlags flag) => _unitDamageManager.GetDamageInfo(flag);
    void Awake()
    {
        _unitDamageManager = new UnitDamageInfoManager(Managers.Data.Unit.DamageInfoByFlag);
        Init();
    }
    protected virtual void Init() { }

    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.AddUnitDamageValue(_unitDamageManager, flag, value, changeStatType);
        photonView.RPC(nameof(AddUnitDamageValue_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, flag, value, changeStatType);
    }

    public void AddUnitDamageValue(UnitColor color, int value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.AddUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(AddUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)color, value, changeStatType);
    }

    public void ScaleUnitDamageValue(UnitColor color, float value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (flag) => flag.UnitColor == color, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithColor_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, (byte)color, value, changeStatType);
    }

    public void ScaleUnitDamageValue(float value, UnitStatType changeStatType)
    {
        _unitDamageInfoChanger.ScaleUnitDamageValue(_unitDamageManager, (x) => true, value, changeStatType);
        photonView.RPC(nameof(ScaleUnitDamageValueWithAll_RPC), RpcTarget.MasterClient, PlayerIdManager.Id, value, changeStatType);
    }

    // 이 함수들은 서버에서 구현
    [PunRPC] protected virtual void AddUnitDamageValue_RPC(byte id, UnitFlags flag, int value, UnitStatType changeStatType){ }
    [PunRPC] protected virtual void AddUnitDamageValueWithColor_RPC(byte id, byte color, int value, UnitStatType changeStatType){ }
    [PunRPC] protected virtual void ScaleUnitDamageValueWithColor_RPC(byte id, byte color, float value, UnitStatType changeStatType) { }
    [PunRPC] protected virtual void ScaleUnitDamageValueWithAll_RPC(byte id, float value, UnitStatType changeStatType) { }
}


public class ServerUnitUpgradeController : UnitUpgradeController
{
    ServerManager _serverManager;
    protected override void Init() => _serverManager = MultiServiceMidiator.Server;

    [PunRPC]
    protected override void AddUnitDamageValue_RPC(byte id, UnitFlags flag, int value, UnitStatType changeStatType)
    {
        _serverManager.AddUnitDamageValue(id, flag, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, flag);
    }

    [PunRPC]
    protected override void AddUnitDamageValueWithColor_RPC(byte id, byte color, int value, UnitStatType changeStatType)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        _serverManager.AddUnitDamageValue(id, conditon, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, conditon);
    }

    [PunRPC]
    protected override void ScaleUnitDamageValueWithColor_RPC(byte id, byte color, float value, UnitStatType changeStatType)
    {
        Func<UnitFlags, bool> conditon = (flag) => flag.UnitColor == (UnitColor)color;
        _serverManager.ScaleUnitDamageValue(id, conditon, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, conditon);
    }

    [PunRPC]
    protected override void ScaleUnitDamageValueWithAll_RPC(byte id, float value, UnitStatType changeStatType)
    {
        _serverManager.ScaleUnitDamageValue(id, (x) => true, value, changeStatType);
        UpdateCurrentUnitDamageInfo(id, (x) => true);
    }

    void UpdateCurrentUnitDamageInfo(byte id, UnitFlags targetFlag) => UpdateCurrentUnitDamageInfo(id, (flag) => flag == targetFlag);

    void UpdateCurrentUnitDamageInfo(byte id, Func<UnitFlags, bool> condition)
        => _serverManager.GetUnits(id)
            .Where(x => condition(x.UnitFlags))
            .ToList()
            .ForEach(x => x.UpdateDamageInfo(_serverManager.GetUnitDamageInfoManager(id).GetDamageInfo(x.UnitFlags)));
}
