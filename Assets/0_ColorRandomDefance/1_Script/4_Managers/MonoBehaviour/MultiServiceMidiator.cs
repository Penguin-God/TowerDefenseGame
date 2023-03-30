using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class MultiServiceMidiator : SingletonPun<MultiServiceMidiator>
{
    static ServerManager _server;
    static UnitUpgradeController _unitUpgrade;

    public static ServerManager Server => _server;
    public static UnitUpgradeController UnitUpgrade => _unitUpgrade;

    protected override void Init()
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
    public IEnumerable<T> Services => _services;
}
