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
public class UI_UnitWindowData
{
    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] List<UnitFlags> _combineUnitFalgs;
    [SerializeField] string _description;

    public UnitFlags UnitFlags => _unitFlags;
    public IReadOnlyList<CombineData> CombineDatas => _combineUnitFalgs.Select(x => Multi_Managers.Data.CombineDataByUnitFlags[x]).ToList();
    public string Description => _description;
    public void SetDescription() => _description = _description.Replace("\\n", "\n");
}

[Serializable]
public class UI_UnitWindowDatas : ICsvLoader<UnitFlags, UI_UnitWindowData>
{
    public Dictionary<UnitFlags, UI_UnitWindowData> MakeDict(string csv)
    {
        List<UI_UnitWindowData> datas = CsvUtility.GetEnumerableFromCsv<UI_UnitWindowData>(csv).ToList();
        datas.ForEach(x => x.SetDescription());
        return datas.ToDictionary(x => x.UnitFlags, x => x);
    }
}

[Serializable]
public struct UI_RandomShopGoodsData
{
    [SerializeField] string name;
    [SerializeField] int goodsType;
    [SerializeField] int grade;
    [SerializeField] string currencyType;
    [SerializeField] int price;
    [SerializeField] string infomation;

    public string Name => name;
    public int GoodsType => goodsType;
    public int Grade => grade;
    public string CurrencyType => currencyType;
    public int Price => price;
    public string Infomation => infomation;
}