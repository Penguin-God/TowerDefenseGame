using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PoolType
{
    Ect,
    Unit,
    Monster,
    Weapon,
    Effect,
}

public class MultiPoolManager
{
    readonly PoolManager _poolManager = new PoolManager();

    public MultiPoolManager()
    {
        _poolManager.Init("MultiPoolManager");
    }

    public Transform CreatePool_InGroup(string path, int count, string groupName, IInstantiater instantiater = null)
    {
        return null;
    }
}
