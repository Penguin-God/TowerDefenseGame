using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/UnitUpgradeShop")]
public class UnitUpgradeShopData : ScriptableObject
{
    public int AddValue;
    public CurrencyData AddValuePriceData;
    public UnitUpgradeGoodsData AddData => new UnitUpgradeGoodsData(UnitUpgradeType.Value, AddValue, AddValuePriceData);

    public int UpScale;
    public CurrencyData UpScalePriceData;
    public UnitUpgradeGoodsData ScaleData => new UnitUpgradeGoodsData(UnitUpgradeType.Scale, UpScale, UpScalePriceData);

    public int MaxUpgradeLevel;
    public int ResetPrice;

    public UnitUpgradeShopData Clone()
    {
        var result = ScriptableObject.CreateInstance<UnitUpgradeShopData>();
        result.AddValue = AddValue;
        result.AddValuePriceData = AddValuePriceData.Cloen();
        result.UpScale = UpScale;
        result.UpScalePriceData = UpScalePriceData.Cloen();
        result.MaxUpgradeLevel = MaxUpgradeLevel;
        result.ResetPrice = ResetPrice;
        return result;
    }
}
