using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Multi_NormalUnitSpawner : Multi_SpawnerBase
{
    public event Action<Multi_TeamSoldier> OnSpawn;
    
    [SerializeField] int[] poolCounts = new int[] { 5, 4, 3, 2 };

    protected override void MasterInit()
    {
        poolCounts = new int[] { 5, 4, 3, 2 };
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

    public void Spawn(UnitFlags flag) => Spawn(flag.ColorNumber, flag.ClassNumber);
    public void Spawn(int unitColor, int unitClass) => Spawn_RPC(GetUnitPath(unitColor, unitClass), GetUnitSpawnPos());
    public void Spawn(UnitFlags flag, int id) => Spawn_RPC(GetUnitPath(flag.ColorNumber, flag.ClassNumber), GetUnitSpawnPos(id), id);
    public void Spawn(UnitFlags flag, Vector3 spawnPos, Quaternion rotation, int id)
        => Spawn_RPC(GetUnitPath(flag.ColorNumber, flag.ClassNumber), spawnPos, rotation, id);

    string GetUnitPath(int unitColor, int unitClass) => PathBuilder.BuildUnitPath(new UnitFlags(unitColor, unitClass));

    Vector3 GetUnitSpawnPos() => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon();
    Vector3 GetUnitSpawnPos(int id) => Multi_WorldPosUtility.Instance.GetUnitSpawnPositon(id);

    [PunRPC]
    protected override GameObject BaseSpawn(string path, Vector3 spawnPos, Quaternion rotation, int id)
    {
        var unit = base.BaseSpawn(path, spawnPos, rotation, id).GetComponent<Multi_TeamSoldier>();
        unit.Spawn();
        OnSpawn?.Invoke(unit);
        return null;
    }
}
