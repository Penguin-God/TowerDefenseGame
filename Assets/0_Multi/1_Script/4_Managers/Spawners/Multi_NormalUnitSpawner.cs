using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
struct UnitPoolingData
{
    public GameObject[] gos;
    public string folderName;
    public int poolingCount;
}

public class Multi_NormalUnitSpawner : Multi_SpawnerBase
{
    public event Action<Multi_TeamSoldier> OnSpawn;
    public event Action<Multi_TeamSoldier> OnDead;

    [SerializeField] UnitPoolingData swordmanPoolData;
    [SerializeField] UnitPoolingData archerPoolData;
    [SerializeField] UnitPoolingData spearmanPoolData;
    [SerializeField] UnitPoolingData magePoolData;

    public override void Init()
    {
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

    // TODO : 구현하기
    void SetUnit(Multi_TeamSoldier unit)
    {
        
    }
}
