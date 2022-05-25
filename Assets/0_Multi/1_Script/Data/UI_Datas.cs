using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct UI_UnitTrackerData
{
    [SerializeField] UnitFlags _UnitFlags;
    [SerializeField] Sprite _icon;
    [SerializeField] Color _backGroundColor;

    public UI_UnitTrackerData(UnitFlags unitNum, Sprite icon, Color color)
    {
        _UnitFlags = unitNum;
        _icon = icon;
        _backGroundColor = color;
    }

    public UnitFlags UnitFlags => _UnitFlags;
    public Sprite Icon => _icon;
    public Color BackGroundColor => _backGroundColor;
}

[Serializable]
public struct UI_UnitWindowData
{
    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] CombineData _combineData; // TODO : 2개 이상 조합을 지원하는 경우도 있기에 List로 만들기
    [SerializeField] string _description;

    public UI_UnitWindowData(UnitFlags unitFlags, CombineData combineData, string description)
    {
        _unitFlags = unitFlags;
        _combineData = combineData;
        _description = description;
    }

    public UnitFlags UnitFlags => _unitFlags;
    public CombineData CombineData => _combineData;
    public UnitFlags CombineUnitFlags => _combineData.UnitFlags;
    public string CombineUnitName => _combineData.KoearName;
    public string Description => _description;
}

[Serializable]
public class UI_UnitWindowDatas : ILoader<UnitFlags, UI_UnitWindowData>
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
}