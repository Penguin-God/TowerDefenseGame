using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class MultiServiceMidiator : SingletonPun<MultiServiceMidiator>
{
    // 마스터 전용
    static ServerManager _server;
    static UnitUpgradeController _unitUpgrade;
    
    public static ServerManager Server => _server;
    public static UnitUpgradeController UnitUpgrade => _unitUpgrade;
    
    public override void Init()
    {
        base.Init();
        _server = new ServerManager(Managers.Data.Unit.DamageInfoByFlag);
        _unitUpgrade = (PhotonNetwork.IsMasterClient) ? gameObject.AddComponent<ServerUnitUpgradeController>() : gameObject.AddComponent<UnitUpgradeController>();
    }
}


public class MultiData<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiData(Func<T> createService) => _services = _services.Select(x => createService()).ToArray();

    public T GetData(byte id) => _services[id];
    public void SetData(byte id, T data) => _services[id] = data;
    public IEnumerable<T> Services => _services;
}

public static class MultiDataFactory
{
    public static MultiData<T> CreateMultiData<T>() where T : new() => new MultiData<T>(() => new T());
}