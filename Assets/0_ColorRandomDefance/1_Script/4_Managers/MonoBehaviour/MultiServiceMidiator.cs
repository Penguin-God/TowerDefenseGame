using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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


public class MultiData<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiData(Func<T> createService) => _services = _services.Select(x => createService()).ToArray();

    public T GetData(byte id) => _services[id];
    public IEnumerable<T> Services => _services;
}
