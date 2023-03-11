using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiServiceMidiator
{
       
}


public class MultiService<T>
{
    T[] _services = new T[PlayerIdManager.MaxPlayerCount];
    public MultiService(System.Func<T> createService)
    {
        for (int i = 0; i < _services.Length; i++)
            _services[i] = createService();
    }

    public T GetServiece() => _services[PlayerIdManager.Id];
    public T GetServiece(byte id) => _services[id];
}
