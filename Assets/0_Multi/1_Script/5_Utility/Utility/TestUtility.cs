using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TestUtility : MonoBehaviour
{
    [SerializeField] List<UnitStat> stats;

    [ContextMenu("Test")]
    void Test()
    {
        stats.Clear();
        foreach (var item in Multi_Managers.Data.UnitStatByFlag)
        {
            stats.Add(item.Value);
        }
    }
}