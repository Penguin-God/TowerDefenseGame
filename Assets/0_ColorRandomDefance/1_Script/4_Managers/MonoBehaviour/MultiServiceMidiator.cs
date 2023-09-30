using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;
using System.Linq;

public class MultiServiceMidiator : SingletonPun<MultiServiceMidiator>
{
    // 마스터 전용
    static ServerManager _server;
    public static ServerManager Server => _server;
    
    public override void Init()
    {
        base.Init();
        _server = new ServerManager(Managers.Data.Unit.DamageInfoByFlag);
    }
}


public class MultiData<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiData() { }
    public MultiData(Func<T> createService) => _services = _services.Select(x => createService()).ToArray();

    public T GetData(byte id) => _services[id];
    public void SetData(byte id, T data) => _services[id] = data;
    public IEnumerable<T> Services => _services;
}

public static class WorldDataFactory
{
    public static MultiData<T> CreateWorldData<T>() where T : new() => new MultiData<T>(() => new T());
}