using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType
{
    Arrow,
    Spear,
    Mageball,
    MageSkill,
    Arrows,
    Spears,
    Mageballs,
    MageSkills,
}

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    [SerializeField] FolderPoolingData[] allWeapons;

    [SerializeField] FolderPoolingData arrowPoolData;
    [SerializeField] FolderPoolingData spearPoolData;
    [SerializeField] FolderPoolingData mageballPoolData;
    [SerializeField] FolderPoolingData mageSkillPoolData;

    protected override void Init()
    {
        SetAllWeapons();

        InitWeapons(arrowPoolData.gos, arrowPoolData.folderName, arrowPoolData.poolingCount);
        InitWeapons(spearPoolData.gos, spearPoolData.folderName, spearPoolData.poolingCount);
        InitWeapons(mageballPoolData.gos, mageballPoolData.folderName, mageballPoolData.poolingCount);
        InitWeapons(mageSkillPoolData.gos, mageSkillPoolData.folderName, mageSkillPoolData.poolingCount);
    }

    void InitWeapons(GameObject[] gos, string folderName, int count)
    {
        for (int i = 0; i < gos.Length; i++)
            CreatePool_InGroup(gos[i], BuildPath(_rootPath, folderName, gos[i]), count);
    }

    void SetAllWeapons()
    {
        allWeapons = new FolderPoolingData[4];
        allWeapons[0] = arrowPoolData;
        allWeapons[1] = spearPoolData;
        allWeapons[2] = mageballPoolData;
        allWeapons[3] = mageSkillPoolData;
    }

    public GameObject Spawn(GameObject go, Vector3 spawnPos) => Multi_Managers.Resources.PhotonInsantiate(go, spawnPos);
    public GameObject Spawn(WeaponType weaponType, string weaponName, Vector3 spawnPos)
        => Multi_Managers.Resources.PhotonInsantiate(BuildPath(weaponType, weaponName), spawnPos, Multi_Data.instance.Id);

    string BuildPath(WeaponType type, string weaponName) => BuildPath(_rootPath, Enum.GetName(typeof(WeaponType), type), weaponName);
}
