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

    protected void CreatePoolGroup(GameObject go, string path, int count) => Multi_Managers.Pool.CreatePool_InGroup(go, path, count, _rootName, SetPoolObj);

    protected virtual void SetPoolObj(GameObject go) { }

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