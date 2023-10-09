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
            // { UnitClass.Archer, 5 },
            // { UnitClass.Spearman, 1 },
            // { UnitClass.Mage, 0 },
        };

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (var classCountPair in unitClassByWeaponPoolingCount)
                CreatePoolGroup(PathBuilder.BuildUnitWeaponPath(new UnitFlags(color, classCountPair.Key)), classCountPair.Value);
        }

        CreateMageSkillPool();
    }

    void CreateMageSkillPool()
    {
        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            if (color == UnitColor.White) continue;
            CreatePoolGroup(PathBuilder.BuildMageSkillEffectPath(color), 0);
        }
    }
}

public class WeaponPoolCreator
{
    public void InitPool() => CreateWeaponsPool();
    string PoolGroupName => "Weapons";

    void CreateWeaponsPool()
    {
        foreach (string path in Enum.GetValues(typeof(UnitColor)).Cast<UnitColor>().Select(x => CreatePath(new UnitFlags(x, UnitClass.Archer))))
            Managers.Pool.CreatePool_InGroup(path, 5, PoolGroupName);

        foreach (string path in Enum.GetValues(typeof(UnitColor)).Cast<UnitColor>().Select(x => CreatePath(new UnitFlags(x, UnitClass.Spearman))))
            Managers.Pool.CreatePool_InGroup(path, 1, PoolGroupName);

        foreach (string path in Enum.GetValues(typeof(UnitColor)).Cast<UnitColor>().Select(x => CreatePath(new UnitFlags(x, UnitClass.Mage))))
            Managers.Pool.CreatePool_InGroup(path, 0, PoolGroupName);
    }

    string CreatePath(UnitFlags flag) => $"Prefabs/{new ResourcesPathBuilder().BuildUnitWeaponPath(flag)}";
}

public class EffectPoolInitializer
{
    protected string PoolGroupName => "Effects";
    public void InitPool()
    {
        foreach (var data in CsvUtility.CsvToArray<EffectData>(Managers.Resources.Load<TextAsset>("Data/EffectData").text))
        {
            switch (data.EffectType)
            {
                case EffectType.GameObject: Managers.Pool.CreatePool_InGroup(data.Path, 2, PoolGroupName); break;
            }
        }
    }
}
