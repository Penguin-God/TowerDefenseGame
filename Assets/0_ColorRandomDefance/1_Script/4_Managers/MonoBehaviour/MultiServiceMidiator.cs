using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiServiceMidiator : SingletonPun<MultiServiceMidiator>
{
    static GameManagerController _game;

    public static GameManagerController Game => _game;

    protected override void Init()
    {
        base.Init();
        _game = gameObject.AddComponent<GameManagerController>();
    }
}


public class MultiManager<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiManager(System.Func<T> createService)
    {
        for (int i = 0; i < _services.Length; i++)
            _services[i] = createService();
    }

    public T GetServiece() => _services[PlayerIdManager.Id];
    public T GetServiece(byte id) => _services[id];
    public IEnumerable<T> Services => _services;
}
