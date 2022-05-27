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
    readonly string UnitNameDataPath = "Assets/0_Multi/Resources/Data/UnitNameData.json";
    readonly string CombineDataPath = "Assets/0_Multi/Resources/Data/CombineDatas.json";

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
                if (Multi_Managers.Data.CombineDataByUnitFlags.TryGetValue(new UnitFlags(i, k + 1), out CombineData combineData))
                {
                    unitWindowDatas.Add(new UI_UnitWindowData(new UnitFlags(i, k), combineData, "test33333"));
                }
                else
                {
                    print(k);
                    unitWindowDatas.Add(new UI_UnitWindowData(new UnitFlags(i, k), new CombineData(), "test33333"));
                }
                // unitWindowDatas.Add(new UI_UnitWindowData(new UnitFlags(i, k), new UnitFlags(i, (k < 3) ? k + 1 : -1), "test33333"));
            }
        }

        CreateFile(UnitWindowUIDataPath, GetSerialization(unitWindowDatas));
    }

    [ContextMenu("Create Unit Name Data")]
    void CreateUnitNameFile()
    {
        List<UnitNameData> unitNameDatas = new List<UnitNameData>();
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                unitNameDatas.Add(new UnitNameData(i, k, "프리펩 이름", "한국어 이름"));
            }
        }

        CreateFile(UnitNameDataPath, GetSerialization(unitNameDatas));
    }

    [ContextMenu("Create Combine Data")]
    void CreateCombineFile()
    {
        List<CombineData> _combineDatas = new List<CombineData>();
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                List<CombineCondition> conditions = new List<CombineCondition>();
                conditions.Add(new CombineCondition(0,0,3));
                _combineDatas.Add(new CombineData(i, k, "빨간 궁수", conditions));
            }
        }

        CreateFile(CombineDataPath, GetSerialization(_combineDatas));
    }

    void CreateFile<T>(string path, Serialization<T> serialization)
    {
        File.WriteAllText(path, JsonUtility.ToJson(serialization, true));
        print("Done");
    }

    Serialization<T> GetSerialization<T>(List<T> target)
        => new Serialization<T>(target);
}
