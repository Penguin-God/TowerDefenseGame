using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public abstract class Multi_SpawnerBase : MonoBehaviourPun, IInstantiater
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

    [SerializeField] protected string _rootName;
    public GameObject Instantiate(string path)
    {
        var result = Managers.Multi.Instantiater.Instantiate(path);
        SetPoolObj(result);
        return result;
    }
    protected virtual void SetPoolObj(GameObject go) { }

    protected void Spawn_RPC(string path, Vector3 spawnPos, int id) 
        => photonView.RPC("BaseSpawn", RpcTarget.MasterClient, path, spawnPos, Quaternion.identity, id);

    [PunRPC]
    protected virtual GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
        => Managers.Multi.Instantiater.PhotonInstantiate(path, spawnPos, rotation, id);
}

public abstract class PhotonObjectPoolInitializerBase : IInstantiater
{
    protected readonly ResourcesPathBuilder PathBuilder = new ResourcesPathBuilder();

    public abstract void InitPool();
    protected abstract string PoolGroupName { get; }
    protected void CreatePoolGroup(string path, int count) => Managers.Pool.CreatePool_InGroup(path, count, PoolGroupName, this);

    public GameObject Instantiate(string path) => Managers.Multi.Instantiater.Instantiate(path);
}

public class UnitPoolInitializer : PhotonObjectPoolInitializerBase
{
    protected override string PoolGroupName => "Units";
    public override void InitPool()
    {
        int[] poolCounts = new int[] { 5, 4, 3, 2 };
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
    readonly int POOL_OBJECT_COUNT = 50;
    protected override string PoolGroupName => "NormalEnemys";
    public override void InitPool()
    {
        for (int i = 0; i < 4; i++)
            CreatePoolGroup(PathBuilder.BuildMonsterPath(i), POOL_OBJECT_COUNT);
    }

}

public class WeaponPoolInitializer : PhotonObjectPoolInitializerBase
{
    public override void InitPool() => CreateWeaponsPool();
    protected override string PoolGroupName => "Weapons";

    void CreateWeaponsPool()
    {
        var unitClassByWeaponPoolingCount = new Dictionary<UnitClass, int>()
        {
            { UnitClass.Archer, 20 },
            { UnitClass.Spearman, 2 },
            { UnitClass.Mage, 0 },
        };

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (var classCountPair in unitClassByWeaponPoolingCount)
                CreatePoolGroup(PathBuilder.BuildUnitWeaponPath(new UnitFlags(color, classCountPair.Key)), classCountPair.Value);
        }

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            if (color == UnitColor.White) continue;
            CreatePoolGroup(PathBuilder.BuildMageSkillEffectPath(color), 0);
        }
    }
}
