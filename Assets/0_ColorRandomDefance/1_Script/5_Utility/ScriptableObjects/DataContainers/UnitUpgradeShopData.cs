using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/UnitUpgradeShop")]
public class UnitUpgradeShopData : ScriptableObject
{
    public int AddValue;
    public CurrencyData AddValuePriceData;
    public UnitUpgradeGoodsData AddData => new (UnitDamageInfo.CreateDamageInfo(AddValue), UnitUpgradeType.Value, AddValuePriceData);

    public float UpScale;
    public CurrencyData UpScalePriceData;
    public UnitUpgradeGoodsData ScaleData => new (UnitDamageInfo.CreateRateInfo(UpScale), UnitUpgradeType.Scale, UpScalePriceData);

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
