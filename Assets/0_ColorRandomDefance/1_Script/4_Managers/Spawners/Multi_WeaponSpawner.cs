using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FolderPoolingData
{
    public string folderName;
    public GameObject[] gos;
    public int poolingCount;
}

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    [SerializeField] FolderPoolingData[] allWeapons;

    [SerializeField] FolderPoolingData arrowPoolData;
    [SerializeField] FolderPoolingData spearPoolData;
    [SerializeField] FolderPoolingData mageballPoolData;
    [SerializeField] FolderPoolingData mageSkillPoolData;

    protected override void MasterInit()
    {
        SetAllWeapons();

        InitWeapons(arrowPoolData.gos, arrowPoolData.folderName, arrowPoolData.poolingCount);
        InitWeapons(spearPoolData.gos, spearPoolData.folderName, spearPoolData.poolingCount);
        InitWeapons(mageballPoolData.gos, mageballPoolData.folderName, mageballPoolData.poolingCount);
        InitWeapons(mageSkillPoolData.gos, mageSkillPoolData.folderName, mageSkillPoolData.poolingCount);
    }

    [SerializeField] string _rootPath;
    void InitWeapons(GameObject[] gos, string folderName, int count)
    {
        for (int i = 0; i < gos.Length; i++)
            CreatePoolGroup(BuildPath(_rootPath, folderName, gos[i]), count);
    }


    string BuildPath(string rooPath, string folderName, GameObject go) => $"{rooPath}/{folderName}/{go.name}";

    void SetAllWeapons()
    {
        allWeapons = new FolderPoolingData[4];
        allWeapons[0] = arrowPoolData;
        allWeapons[1] = spearPoolData;
        allWeapons[2] = mageballPoolData;
        allWeapons[3] = mageSkillPoolData;
    }

    public GameObject Spawn(string path, Vector3 spawnPos) => Managers.Multi.Instantiater.PhotonInstantiate($"Weapon/{path}", spawnPos);
}
