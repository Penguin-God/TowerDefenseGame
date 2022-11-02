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
        List<UI_UnitWindowData> datas = CsvUtility.CsvToArray<UI_UnitWindowData>(csv).ToList();
        datas.ForEach(x => x.SetDescription());
        return datas.ToDictionary(x => x.UnitFlags, x => x);
    }
}

public enum GoodsLocation
{
    None,
    Left,
    Middle,
    Right,
}

[Serializable]
public struct UI_RandomShopGoodsData
{
    [SerializeField] string _name;
    [SerializeField] GoodsLocation _goodsLocation;
    [SerializeField] int _grade;
    [SerializeField] GameCurrencyType _currencyType;
    [SerializeField] int _price;
    [SerializeField] string _infomation;
    [SerializeField] SellType _sellType;
    [SerializeField] int[] _sellDatas;

    public UI_RandomShopGoodsData
        (string name, GoodsLocation location, int grade, GameCurrencyType currencyType, int price, string info, SellType sellType, int[] sellDatas)
    {
        _name = name;
        _goodsLocation = location;
        _grade = grade;
        _currencyType = currencyType;
        _price = price;
        _infomation = info;
        _sellType = sellType;
        _sellDatas = sellDatas;
    }

    public string Name => _name;
    public GoodsLocation GoodsLocation => _goodsLocation;
    public int Grade => _grade;
    public GameCurrencyType CurrencyType => _currencyType;
    public int Price => _price;
    public string Infomation => _infomation;
    public SellType SellType => _sellType;
    public IReadOnlyList<int> SellDatas => _sellDatas;
}