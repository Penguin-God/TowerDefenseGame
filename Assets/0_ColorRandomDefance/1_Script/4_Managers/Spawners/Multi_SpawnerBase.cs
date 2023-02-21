using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public abstract class Multi_SpawnerBase : MonoBehaviourPun, IInstantiater
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        MasterInit();
    }

    protected virtual void MasterInit() { }


    [SerializeField] protected string _rootName;
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

public class PhotonObjectPoolInitializer : IInstantiater
{
    readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

    public void Init()
    {
        CreateWeaponsPool();
        CreateUnitsPool();
        CreateMonstersPool();
    }

    protected void CreatePoolGroup(string path, string poolGroupName, int count) => Managers.Pool.CreatePool_InGroup(path, count, poolGroupName, this);
    public GameObject Instantiate(string path) => Managers.Multi.Instantiater.Instantiate(path);

    void CreateUnitsPool()
    {

    }

    void CreateMonstersPool()
    {

    }

    void CreateWeaponsPool()
    {
        const string ROOT_NAME = "Weapons";
        var unitClassByWeaponPoolingCount = new Dictionary<UnitClass, int>()
        {
            { UnitClass.Archer, 20 },
            { UnitClass.Spearman, 2 },
            { UnitClass.Mage, 0 },
        };

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (var classCountPair in unitClassByWeaponPoolingCount)
                CreatePoolGroup(PathBuilder.BuildUnitWeaponPath(new UnitFlags(color, classCountPair.Key)), ROOT_NAME, classCountPair.Value);
        }

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            if (color == UnitColor.White) continue;
            CreatePoolGroup(PathBuilder.BuildMageSkillEffectPath(color), ROOT_NAME, 0);
        }
    }
}
