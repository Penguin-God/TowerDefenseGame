using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public enum SpawnerType
{
    NormalEnemy,
    BossEnemy,
    TowerEnemy,
    NormalUnit,
}

public abstract class Multi_SpawnerBase : MonoBehaviour
{
    [SerializeField] protected string _rootName;
    [SerializeField] protected string _rootPath;

    protected PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        Init();
        if (PhotonNetwork.IsMasterClient == false) return;
        MasterInit();
    }

    protected virtual void Init() { }

    protected virtual void MasterInit() { }

    protected T[] CreatePool<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool(go, path, count).GetComponentsInChildren<T>(true).ToArray();

    protected T[] CreatePool_InGroup<T>(GameObject go, string path, int count) where T : Component
        => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName).GetComponentsInChildren<T>(true);

    protected Transform CreatePool_InGroup(GameObject go, string path, int count)
        => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName);

    protected Transform CreatePool_InGroup(GameObject go, int count)
        => Multi_Managers.Pool.CreatePool_InGroup(go, BuildPath(_rootPath, go), count, _rootName);

    protected void Spawn_RPC(string path, Vector3 spawnPos, int id) 
        => pv.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, id);
    protected void Spawn_RPC(string path, Vector3 spawnPos) 
        => pv.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, Multi_Data.instance.Id);
    protected void Spawn_RPC(string path, Vector3 spawnPos, Quaternion rotation, int id)
        => pv.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, rotation, id);

    [PunRPC]
    protected virtual GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id) 
        => Multi_Managers.Resources.PhotonInsantiate(path, spawnPos, rotation, id);



    public string BuildPath(string rooPath, GameObject go) => $"{rooPath}/{go.name}";
    public string BuildPath(string rooPath, string folderName, GameObject go) => $"{rooPath}/{folderName}/{go.name}";
    public string BuildPath(string rooPath, string folderName, string name) => $"{rooPath}/{folderName}/{name}";
}