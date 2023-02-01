using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Multi_SpawnerBase : MonoBehaviourPun, IInstantiater
{
    [SerializeField] protected string _rootName;

    void Start()
    {
        Init();
        if (PhotonNetwork.IsMasterClient == false) return;
        MasterInit();
    }

    protected virtual void Init() { }

    protected virtual void MasterInit() { }

    protected void CreatePoolGroup(string path, int count) => Managers.Pool.CreatePool_InGroup(path, count, _rootName, this);

    public GameObject Instantiate(string path)
    {
        var result = Managers.Multi.Instantiater.Instantiate(path);
        SetPoolObj(result);
        return result;
    }

    protected virtual void SetPoolObj(GameObject go) { }

    protected void Spawn_RPC(string path, Vector3 spawnPos, int id) 
        => photonView.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, id);
    protected void Spawn_RPC(string path, Vector3 spawnPos) 
        => photonView.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, Multi_Data.instance.Id);
    protected void Spawn_RPC(string path, Vector3 spawnPos, Quaternion rotation, int id)
        => photonView.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, rotation, id);

    [PunRPC]
    protected virtual GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
        => Managers.Multi.Instantiater.PhotonInstantiate(path, spawnPos, rotation, id);
}