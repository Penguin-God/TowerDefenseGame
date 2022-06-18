﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    void LoadCSV(TextAsset csv);
}

public interface ICsvLoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(string csv);
}

public class Multi_DataManager
{
    #region UI Data
    Dictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg = new Dictionary<UnitFlags, CombineCondition>();
    public IReadOnlyDictionary<UnitFlags, CombineCondition> CombineConditionByUnitFalg => _combineConditionByUnitFalg;

    Dictionary<UnitFlags, CombineData> _combineDataByUnitFlags = new Dictionary<UnitFlags, CombineData>();
    public IReadOnlyDictionary<UnitFlags, CombineData> CombineDataByUnitFlags => _combineDataByUnitFlags;

    Dictionary<UnitFlags, UI_UnitWindowData> _unitWindowDataByUnitFlags = new Dictionary<UnitFlags, UI_UnitWindowData>();
    public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _unitWindowDataByUnitFlags;
    #endregion UI Data End

    Dictionary<UnitFlags, UnitNameData> _unitNameDataByUnitFlags = new Dictionary<UnitFlags, UnitNameData>();
    public IReadOnlyDictionary<UnitFlags, UnitNameData> UnitNameDataByUnitFlags => _unitNameDataByUnitFlags;

    public void Init()
    {
        _combineDataByUnitFlags.Clear();
        _unitWindowDataByUnitFlags.Clear();
        _unitNameDataByUnitFlags.Clear();

        _combineDataByUnitFlags = MakeCsvDict<CombineDatas, UnitFlags, CombineData>("CombineData");
        _combineConditionByUnitFalg = MakeCsvDict<CombineConditions, UnitFlags, CombineCondition>("CombineConditionData");
        _unitWindowDataByUnitFlags = MakeCsvDict<UI_UnitWindowDatas, UnitFlags, UI_UnitWindowData>("UI_UnitWindowData");
    }

    Dictionary<Key, Value> MakeCsvDict<ICsvLoader, Key, Value>(string path) where ICsvLoader : ICsvLoader<Key, Value>, new()
    {
        ICsvLoader loder = new ICsvLoader();
        return loder.MakeDict(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}").text);
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Multi_Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadCSVa<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>, new()
    {
        Loader loader = new Loader();
        loader.LoadCSV(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}"));
        return loader;
    }
}
