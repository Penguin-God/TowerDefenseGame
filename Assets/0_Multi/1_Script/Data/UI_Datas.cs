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
    public IReadOnlyList<UnitFlags> CombineUnitFlags => _combineUnitFalgs;
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

public enum GoodsLocation
{
    Left,
    Middle,
    Right,
}

[Serializable]
public struct UI_RandomShopGoodsData
{
    [SerializeField] string name;
    [SerializeField] GoodsLocation goodsLocation;
    [SerializeField] int grade;
    [SerializeField] GameCurrencyType currencyType;
    [SerializeField] int price;
    [SerializeField] string infomation;
    [SerializeField] SellType sellType;
    [SerializeField] int[] sellDatas;

    public string Name => name;
    public GoodsLocation GoodsLocation => goodsLocation;
    public int Grade => grade;
    public GameCurrencyType CurrencyType => currencyType;
    public int Price => price;
    public string Infomation => infomation;
    public SellType SellType => sellType;
    public IReadOnlyList<int> SellDatas => sellDatas;
}