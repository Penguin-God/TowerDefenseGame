using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiServiceMidiator
{
       
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
