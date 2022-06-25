using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct UI_UnitTrackerData
{
    [SerializeField] UnitFlags _UnitFlags;
    [SerializeField] Sprite _icon;
    [SerializeField] Color _backGroundColor;
    [SerializeField] string _unitClassName;

    public UI_UnitTrackerData(UnitFlags unitNum, Sprite icon, Color color, string unitClassName)
    {
        _UnitFlags = unitNum;
        _icon = icon;
        _backGroundColor = color;
        _unitClassName = unitClassName;
    }

    public UnitFlags UnitFlags => _UnitFlags;
    public Sprite Icon => _icon;
    public Color BackGroundColor => _backGroundColor;
    public string UnitClassName => _unitClassName;
}

[Serializable]
public struct UI_UnitWindowData
{
    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] List<UnitFlags> _combineUnitFalgs;
    [SerializeField] string _description;

    public UnitFlags UnitFlags => _unitFlags;
    public IReadOnlyList<CombineData> CombineDatas => _combineUnitFalgs.Select(x => Multi_Managers.Data.CombineDataByUnitFlags[x]).ToList();
    public string Description => _description;
}

[Serializable]
public class UI_UnitWindowDatas : ILoader<UnitFlags, UI_UnitWindowData>, ICsvLoader<UnitFlags, UI_UnitWindowData>
{
    [SerializeField] List<UI_UnitWindowData> SerializtionDatas = new List<UI_UnitWindowData>();

    public Dictionary<UnitFlags, UI_UnitWindowData> MakeDict()
    {
        Dictionary<UnitFlags, UI_UnitWindowData> dict = new Dictionary<UnitFlags, UI_UnitWindowData>();

        foreach (UI_UnitWindowData data in SerializtionDatas)
            dict.Add(data.UnitFlags, data);

        return dict;
    }


    public void LoadCSV(TextAsset csv)
    {
        throw new NotImplementedException();
    }

    public Dictionary<UnitFlags, UI_UnitWindowData> MakeDict(string csv)
    {
        return CsvUtility.GetEnumerableFromCsv<UI_UnitWindowData>(csv).ToDictionary(x => x.UnitFlags, x => x);
    }
}