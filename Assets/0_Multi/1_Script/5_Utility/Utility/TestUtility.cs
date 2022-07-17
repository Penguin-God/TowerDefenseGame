using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class LookArray
{
    public string[] paths;

    public LookArray(string[] pathss)
    {
        paths = pathss;
    }
}

public class TestUtility : MonoBehaviour
{
    [SerializeField] List<LookArray> Paths;

    [ContextMenu("Test")]
    void Test()
    {
        foreach (var item in Multi_Managers.Data.WeaponDataByUnitFlag)
        {
            Paths.Add(new LookArray(item.Value.Paths.ToArray()));
            print("a");
        }
    }

    void Start()
    {

    }
}