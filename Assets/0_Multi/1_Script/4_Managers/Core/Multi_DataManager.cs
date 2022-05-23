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
    Dictionary<UnitFlags, CombineData> _combineDataByUnitFlags = new Dictionary<UnitFlags, CombineData>();
    public IReadOnlyDictionary<UnitFlags, CombineData> CombineDataByUnitFlags => _combineDataByUnitFlags;

    Dictionary<UnitFlags, UI_UnitWindowData> _unitWindowDataByUnitFlags = new Dictionary<UnitFlags, UI_UnitWindowData>();
    public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _unitWindowDataByUnitFlags;

    Dictionary<UnitFlags, UnitNameData> _unitNameDataByUnitFlags = new Dictionary<UnitFlags, UnitNameData>();
    public IReadOnlyDictionary<UnitFlags, UnitNameData> UnitNameDataByUnitFlags => _unitNameDataByUnitFlags;

    public void Init()
    {
        _unitWindowDataByUnitFlags.Clear();
        _combineDataByUnitFlags.Clear();
        _unitNameDataByUnitFlags.Clear();
        //_combineDataByUnitFlags = LoadCSV<CombineDatas, UnitFlags, CombineData>("CombineData_CSV").MakeDict();
        _unitWindowDataByUnitFlags = LoadJson<UI_UnitWindowDatas, UnitFlags, UI_UnitWindowData>("UnitWindowUIDatas").MakeDict();
        _unitNameDataByUnitFlags = LoadJson<UnitNameDatas, UnitFlags, UnitNameData>("UnitNameData").MakeDict();
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
