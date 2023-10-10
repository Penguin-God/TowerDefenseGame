using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public abstract class Multi_SpawnerBase : MonoBehaviourPun
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    protected virtual void SetSpawnObj(GameObject go) { }

    protected void Spawn_RPC(string path, Vector3 spawnPos, int id) 
        => photonView.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, id);

    [PunRPC]
    protected virtual GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
        => Managers.Multi.Instantiater.PhotonInstantiate(path, spawnPos, rotation, id);
}

public abstract class PhotonObjectPoolInitializerBase : IInstantiater
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();
    readonly PoolManager _poolManager;
    public PhotonObjectPoolInitializerBase(PoolManager poolManager) => _poolManager = poolManager;
    public abstract void InitPool();
    protected abstract string PoolGroupName { get; }
    protected void CreatePoolGroup(string path, int count) => _poolManager.CreatePool_InGroup(path, count, PoolGroupName, this);
    public GameObject Instantiate(string path) => Managers.Multi.Instantiater.Instantiate(path);
}

public class UnitPoolInitializer : PhotonObjectPoolInitializerBase
{
    public UnitPoolInitializer(PoolManager poolManager) : base(poolManager) {}

    protected override string PoolGroupName => "Units";
    public override void InitPool()
    {
        int[] poolCounts = new int[] { 4, 2, 1, 0 };
        CreatePool(UnitClass.Swordman, poolCounts[0]);
        CreatePool(UnitClass.Archer, poolCounts[1]);
        CreatePool(UnitClass.Spearman, poolCounts[2]);
        CreatePool(UnitClass.Mage, poolCounts[3]);
    }

    void CreatePool(UnitClass unitClass, int count)
    {
        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            CreatePoolGroup(PathBuilder.BuildUnitPath(new UnitFlags(color, unitClass)), count);
    }
}

public class MonsterPoolInitializer : PhotonObjectPoolInitializerBase
{
    readonly int POOL_OBJECT_COUNT = 20;
    public MonsterPoolInitializer(PoolManager poolManager) : base(poolManager) {}

    protected override string PoolGroupName => "NormalEnemys";
    public override void InitPool()
    {
        for (int i = 0; i < 4; i++)
            CreatePoolGroup(PathBuilder.BuildMonsterPath(i), POOL_OBJECT_COUNT);
    }

}

public class WeaponPoolCreator
{
    string PoolGroupName => "Weapons";
    PoolManager _poolManager;
    public void InitPool(PoolManager poolManager)
    {
        _poolManager = poolManager;
        CreateWeaponPool(UnitClass.Archer, 5);
        CreateWeaponPool(UnitClass.Spearman, 1);
        CreateWeaponPool(UnitClass.Mage, 0);
    }

    void CreateWeaponPool(UnitClass unitClass, int count)
    {
        foreach (string path in UnitFlags.AllColors.Select(CreatePath))
            _poolManager.CreatePool_InGroup(path, count, PoolGroupName);

        string CreatePath(UnitColor color) => $"Prefabs/{new ResourcesPathBuilder().BuildUnitWeaponPath(new UnitFlags(color, unitClass))}";
    }
}

public class EffectPoolInitializer
{
    protected string PoolGroupName => "Effects";
    public void InitPool(PoolManager poolManager)
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            if(data.EffectType == EffectType.GameObject)
                poolManager.CreatePool_InGroup(data.Path, 2, PoolGroupName);
        }
    }
}
