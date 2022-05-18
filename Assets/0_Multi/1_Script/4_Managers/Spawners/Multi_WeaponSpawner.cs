using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Arrow,
    Spear,
    Mageball,
    MageSkill,
}

public class Multi_WeaponSpawner : Multi_SpawnerBase
{
    [SerializeField] FolderPoolingData[] allWeapons;

    [SerializeField] FolderPoolingData arrowPoolData;
    [SerializeField] FolderPoolingData spearPoolData;
    [SerializeField] FolderPoolingData mageballPoolData;
    [SerializeField] FolderPoolingData mageSkillPoolData;

    public override void Init()
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


    public GameObject Spawn(GameObject go) => Multi_Managers.Pool.Pop(go);

    public GameObject Spawn(WeaponType weaponType, GameObject go, Vector3 _pos) => Spawn(weaponType, go, _pos);
    public GameObject Spawn(int type, GameObject go, Vector3 _pos)
        => Multi_Managers.Resources.PhotonInsantiate(BuildPath(_rootPath, allWeapons[type].folderName, go), _pos);
}
