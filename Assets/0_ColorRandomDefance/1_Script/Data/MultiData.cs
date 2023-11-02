using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MultiData<T>
{
    const int MaxPlayerCount = 2;
    T[] _services = new T[MaxPlayerCount];
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