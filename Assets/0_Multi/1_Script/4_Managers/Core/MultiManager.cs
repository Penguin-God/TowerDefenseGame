using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiManager
{
    MultiInstantiater _multiInstantiater = new MultiInstantiater();
    public IInstantiater Instantiater => _multiInstantiater;

    public void CreatePoolGroup(IEnumerable<string> paths, int count, string groupName = "")
    {
        foreach (string path in paths)
            Multi_Managers.Pool.CreatePool_InGroup(path, count, groupName, _multiInstantiater);
    }

    class MultiInstantiater : IInstantiater
    {
        public GameObject Instantiate(string path)
        {
            var go = PhotonNetwork.Instantiate(path, Vector3.zero * 1000, Quaternion.identity);
            go.GetOrAddComponent<RPCable>();
            return go;
        }
    }
}
