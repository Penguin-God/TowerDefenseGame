using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FolderPoolingData
{
    public string folderName;
    public GameObject[] gos;
    public int poolingCount;
}

public class Multi_NormalUnitSpawner : Multi_SpawnerBase
{
    public event Action<Multi_TeamSoldier> OnSpawn;
    public event Action<Multi_TeamSoldier> OnDead;

    [SerializeField] FolderPoolingData[] allUnitDatas;

    [SerializeField] FolderPoolingData swordmanPoolData;
    [SerializeField] FolderPoolingData archerPoolData;
    [SerializeField] FolderPoolingData spearmanPoolData;
    [SerializeField] FolderPoolingData magePoolData;

    public override void Init()
    {
        SetAllUnit();

        InitUnits(swordmanPoolData.gos, swordmanPoolData.folderName, swordmanPoolData.poolingCount);
        InitUnits(archerPoolData.gos, archerPoolData.folderName, archerPoolData.poolingCount);
        InitUnits(spearmanPoolData.gos, spearmanPoolData.folderName, spearmanPoolData.poolingCount);
        InitUnits(magePoolData.gos, magePoolData.folderName, magePoolData.poolingCount);
    }

    void InitUnits(GameObject[] gos, string folderName,int count)
    {
        for (int i = 0; i < gos.Length; i++)
        {
            Multi_TeamSoldier[] units = CreatePool_InGroup<Multi_TeamSoldier>(gos[i], BuildPath(_rootPath, folderName, gos[i]), count);

            foreach (var unit in units) SetUnit(unit);
        }
    }


    void SetAllUnit()
    {
        allUnitDatas = new FolderPoolingData[4];
        allUnitDatas[0] = swordmanPoolData;
        allUnitDatas[1] = archerPoolData;
        allUnitDatas[2] = spearmanPoolData;
        allUnitDatas[3] = magePoolData;
    }

    // TODO : 구현하기
    void SetUnit(Multi_TeamSoldier unit)
    {

    }

    // TODO : unitClass를 먼저 오게 하기
    public void Spawn(UnitColor unitColor, UnitClass unitClass) => Spawn((int)unitColor, (int)unitClass);
    // TODO : 추가 풀링헤사 event가 비어있는 유닛들은 스폰할 때 event세팅 해주기
    public void Spawn(int unitColor, int unitClass)
    {
        Multi_Managers.Resources.PhotonInsantiate(
            BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]), // path
            Multi_WorldPosUtility.Instance.GetUnitSpawnPositon() // position
            );
    }

    public void Spawn(int unitColor, int unitClass, Vector3 spawnPos)
    {
        Multi_Managers.Resources.PhotonInsantiate(
            BuildPath(_rootPath, allUnitDatas[unitClass].folderName, allUnitDatas[unitClass].gos[unitColor]), spawnPos);
    }
}
