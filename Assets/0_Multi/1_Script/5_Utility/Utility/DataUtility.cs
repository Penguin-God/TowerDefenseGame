using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class Serialization<T>
{
    public List<T> SerializtionDatas;
    public Serialization(List<T> targets) 
    {
        SerializtionDatas = targets;
    }
}

public class DataUtility : MonoBehaviour
{
    readonly string UnitUumberDataPath = "Assets/0_Multi/Resources/Data/UnitFlagss.json";
    readonly string UnitWindowUIDataPath = "Assets/0_Multi/Resources/Data/UnitWindowUIDatas.json";

    [ContextMenu("Create Unit Numbers Data")]
    void CreateUnitFlagssFile()
    {
        List<UnitFlags> _UnitFlagss = new List<UnitFlags>();
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                _UnitFlagss.Add(new UnitFlags(i, k));
            }
        }

        CreateFile(UnitUumberDataPath, GetSerialization(_UnitFlagss));
    }

    [ContextMenu("Create Unit Window UI Data")]
    void CreateUnitWindowUIFile()
    {
        List<UI_UnitWindowData> unitWindowDatas = new List<UI_UnitWindowData>();
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                unitWindowDatas.Add(new UI_UnitWindowData(new UnitFlags(i, k), new UnitFlags(i, (k < 3) ? k + 1 : -1), "test222"));
            }
        }

        CreateFile(UnitWindowUIDataPath, GetSerialization(unitWindowDatas));
    }

    void CreateFile<T>(string path, Serialization<T> serialization)
    {
        File.WriteAllText(path, JsonUtility.ToJson(serialization, true));
        print("Done");
    }

    Serialization<T> GetSerialization<T>(List<T> target)
        => new Serialization<T>(target);
}
