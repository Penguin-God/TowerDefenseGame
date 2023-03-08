﻿using System.Collections;
using System.Collections.Generic;

public class RPCData<T> where T : new()
{
    Dictionary<int, T> _dict = new Dictionary<int, T>();
    const int MAX_PLAYER_COUNT = 2;

    public RPCData()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
            _dict.Add(i, new T());
    }

    public T Get(int id)
    {
        if (_dict.TryGetValue(id, out T result))
            return result;
        return new T();
    }
    public void Set(int id, T t) => _dict[id] = t;
}
