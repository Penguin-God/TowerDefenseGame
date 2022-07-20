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
    // 조합 조건
    Dictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg = new Dictionary<UnitFlags, CombineCondition>();
    public IReadOnlyDictionary<UnitFlags, CombineCondition> CombineConditionByUnitFalg => _combineConditionByUnitFalg;

    // 조합 정보
    Dictionary<UnitFlags, CombineData> _combineDataByUnitFlags = new Dictionary<UnitFlags, CombineData>();
    public IReadOnlyDictionary<UnitFlags, CombineData> CombineDataByUnitFlags => _combineDataByUnitFlags;

    // 유닛 창 정보
    Dictionary<UnitFlags, UI_UnitWindowData> _unitWindowDataByUnitFlags = new Dictionary<UnitFlags, UI_UnitWindowData>();
    public IReadOnlyDictionary<UnitFlags, UI_UnitWindowData> UnitWindowDataByUnitFlags => _unitWindowDataByUnitFlags;
    #endregion UI Data End

    Dictionary<UnitFlags, WeaponData> _weaponDataByUnitFlag = new Dictionary<UnitFlags, WeaponData>();
    public IReadOnlyDictionary<UnitFlags, WeaponData> WeaponDataByUnitFlag => _weaponDataByUnitFlag;

    Dictionary<string, UnitNameData> _unitNameDataByUnitKoreaName = new Dictionary<string, UnitNameData>();
    public IReadOnlyDictionary<string, UnitNameData> UnitNameDataByUnitKoreaName => _unitNameDataByUnitKoreaName;

    Dictionary<UnitFlags, UnitStat> _unitStatByFlag = new Dictionary<UnitFlags, UnitStat>();
    public UnitStat GetUnitStat(UnitFlags flag) => _unitStatByFlag[flag].GetClone();

    Dictionary<UnitFlags, UnitPassiveStat> _unitPassiveStatByFlag = new Dictionary<UnitFlags, UnitPassiveStat>();
    public IReadOnlyList<float> GetUnitPassiveStats(UnitFlags flag) => _unitPassiveStatByFlag[flag].Stats;

    Dictionary<UnitFlags, MageUnitStat> _mageStatByFlag = new Dictionary<UnitFlags, MageUnitStat>();
    public IReadOnlyDictionary<UnitFlags, MageUnitStat> MageStatByFlag => _mageStatByFlag;


    public void Init()
    {
        Clears();

        // 무조건 가장먼저 실행되어야 함
        _unitNameDataByUnitKoreaName = MakeCsvDict<UnitNameDatas, string, UnitNameData>("UnitData/UnitNameData");

        _mageStatByFlag = MakeCsvDict<MageUnitStats, UnitFlags, MageUnitStat>("UnitData/MageUnitStat");
        _unitPassiveStatByFlag = MakeCsvDict<UnitPassiveStats, UnitFlags, UnitPassiveStat>("UnitData/UnitPassiveStat");
        _unitStatByFlag = MakeCsvDict<UnitStats, UnitFlags, UnitStat>("UnitData/UnitStat");
        _weaponDataByUnitFlag = MakeCsvDict<WeaponDatas, UnitFlags, WeaponData>("UnitData/UnitWeaponData");
        _combineDataByUnitFlags = MakeCsvDict<CombineDatas, UnitFlags, CombineData>("UnitData/CombineData");
        _combineConditionByUnitFalg = MakeCsvDict<CombineConditions, UnitFlags, CombineCondition>("UnitData/CombineConditionData");

        _unitWindowDataByUnitFlags = MakeCsvDict<UI_UnitWindowDatas, UnitFlags, UI_UnitWindowData>("UIData/UI_UnitWindowData");
    }

    void Clears()
    {
        _combineDataByUnitFlags.Clear();
        _unitWindowDataByUnitFlags.Clear();
        _combineConditionByUnitFalg.Clear();
        _unitNameDataByUnitKoreaName.Clear();
        _unitStatByFlag.Clear();
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
}
