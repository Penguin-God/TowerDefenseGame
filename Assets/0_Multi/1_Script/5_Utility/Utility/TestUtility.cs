using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] List<BossData> datas;
    [ContextMenu("Test")]
    void Test()
    {
        datas.Clear();
        foreach (var item in Multi_Managers.Data.BossDataByLevel)
        {
            datas.Add(item.Value);
        }
    }

    [SerializeField] int spawnColorMax;
    [SerializeField] int spawnClassMax;
    [ContextMenu("Unit Spawn")]
    void UnitSpawn()
    {
        for (int i = 0; i <= spawnColorMax; i++)
        {
            for (int j = 0; j <= spawnClassMax; j++)
            {
                Multi_SpawnManagers.NormalUnit.Spawn(i, j);
            }
        }
    }
}