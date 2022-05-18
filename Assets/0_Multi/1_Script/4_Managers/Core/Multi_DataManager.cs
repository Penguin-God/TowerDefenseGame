using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    void LoadCSV(TextAsset csv);
}

public class Multi_DataManager
{
    Dictionary<UnitNumber, CombineData> _combineDataByUnitNumbers = new Dictionary<UnitNumber, CombineData>();
    public IReadOnlyDictionary<UnitNumber, CombineData> CombineDataByUnitNumbers => _combineDataByUnitNumbers;

    public void Init()
    {
        _combineDataByUnitNumbers = LoadCSV<CombineDatas, UnitNumber, CombineData>("CombineData_CSV").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Multi_Managers.Resources.Load<TextAsset>($"Data/{path}");
        Debug.Log(textAsset.text);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadCSV<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>, new()
    {
        Loader loader = new Loader();
        loader.LoadCSV(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}"));
        return loader;
    }
}
