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
    static OppentStatusManager _oppent = new OppentStatusManager();
    static SpawnerController _spawner;

    public static ServerManager Server => _server;
    public static UnitUpgradeController UnitUpgrade => _unitUpgrade;
    public static OppentStatusManager Oppent => _oppent;
    public static SpawnerController Spawner => _spawner;

    public override void Init()
    {
        base.Init();
        _server = new ServerManager(Managers.Data.Unit.DamageInfoByFlag);
        _unitUpgrade = (PhotonNetwork.IsMasterClient) ? gameObject.AddComponent<ServerUnitUpgradeController>() : gameObject.AddComponent<UnitUpgradeController>();
        _spawner = (PhotonNetwork.IsMasterClient) ? gameObject.AddComponent<ServerSpawnerController>() : gameObject.AddComponent<SpawnerController>();
        _spawner.Init(Multi_GameManager.Instance);
        _oppent.Init(new OpponentStatusSynchronizer());
    }
}


public class MultiData<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiData(Func<T> createService) => _services = _services.Select(x => createService()).ToArray();

    public T GetData(byte id) => _services[id];
    public IEnumerable<T> Services => _services;
}
