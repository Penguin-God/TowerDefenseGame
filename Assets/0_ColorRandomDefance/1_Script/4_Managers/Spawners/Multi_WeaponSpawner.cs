using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct FolderPoolingData
{
    public string folderName;
    public GameObject[] gos;
    public int poolingCount;
}

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    [SerializeField] FolderPoolingData mageballPoolData;
    [SerializeField] FolderPoolingData mageSkillPoolData;

    protected override void MasterInit()
    {
        PoolWeapon();

        InitWeapons(mageballPoolData.gos, mageballPoolData.folderName, mageballPoolData.poolingCount);
        InitWeapons(mageSkillPoolData.gos, mageSkillPoolData.folderName, mageSkillPoolData.poolingCount);
    }

    void PoolWeapon()
    {
        var poolWeaponUnitClassArr = new UnitClass[] { UnitClass.Archer, UnitClass.Spearman };
        var unitClassByWeaponPoolingCount = new Dictionary<UnitClass, int>()
        {
            { UnitClass.Archer, 20 },
            { UnitClass.Spearman, 2 },
        };

        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass unitClass in poolWeaponUnitClassArr)
                CreatePoolGroup(PathBuilder.BuildUnitWeaponPath(new UnitFlags(color, unitClass)), unitClassByWeaponPoolingCount[unitClass]);
        }
    }

    [SerializeField] string _rootPath;
    void InitWeapons(GameObject[] gos, string folderName, int count)
    {
        for (int i = 0; i < gos.Length; i++)
            CreatePoolGroup(BuildPath(_rootPath, folderName, gos[i]), count);
    }
    string BuildPath(string rooPath, string folderName, GameObject go) => $"{rooPath}/{folderName}/{go.name}";

    public GameObject Spawn(string path, Vector3 spawnPos) => Managers.Multi.Instantiater.PhotonInstantiate($"Weapon/{path}", spawnPos);
}
