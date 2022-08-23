using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using System;

[Serializable]
class SingleData
{
    [SerializeField] int startGold;
    [SerializeField] int startFood;
    [SerializeField] int startMaxUnitCount;
}

[Serializable]
struct ConstsSingleData
{
    [SerializeField] int maxEnemyCount;
    [SerializeField] int startMaxUnitCount;
}

public class TestUtility : MonoBehaviour
{
    [SerializeField] SingleData data;
    [SerializeField] ConstsSingleData constData;

    [ContextMenu("Test")]
    void Test()
    {
        print(JsonUtility.ToJson(data, true));
        print(JsonUtility.ToJson(constData, true));


    }

    [SerializeField] int spawnColorMax;
    [SerializeField] int spawnClassMax;
    [ContextMenu("범위 안의 Unit Spawn")]
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