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
        Paths.Clear();
        foreach (var item in Multi_Managers.Data.WeaponDataByUnitFlag)
        {
            Paths.Add(new LookArray(item.Value.Paths.ToArray()));
        }

        foreach (LookArray item in Paths)
        {
            foreach (var path in item.paths)
            {
                if (Resources.Load($"Prefabs/Weapon/{path}") == null)
                    print($"no : {path}");
            }
        }

        print("Good");
    }
}