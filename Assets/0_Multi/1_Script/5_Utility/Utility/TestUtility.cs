using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class FloatStats
{
    public float[] stats;

    public FloatStats(IReadOnlyList<float> _stats) => stats = _stats.ToArray();
}

public class TestUtility : MonoBehaviour
{
    [SerializeField] List<FloatStats> stats;
    [SerializeField] IReadOnlyList<int> aa;
    [ContextMenu("Test")]
    void Test()
    {
        stats.Clear();
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