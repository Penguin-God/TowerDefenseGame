using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCData<T> where T : new()
{
    Dictionary<int, T> _dict = new Dictionary<int, T>();

    public RPCData()
    {
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++)
            _dict.Add(i, new T());
    }

    public T Get(int id)
    {
        if (_dict.TryGetValue(id, out T result))
            return result;
        return new T();
    }
    public void Set(int id, T t) => _dict[id] = t;
    public void Set(Component com, T t) => _dict[com.GetComponent<RPCable>().UsingId] = t;
}
