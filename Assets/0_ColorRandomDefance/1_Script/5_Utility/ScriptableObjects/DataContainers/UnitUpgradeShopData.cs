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
    [SerializeField] int _maxUpgradeLevel;

    [SerializeField] CurrencyData _maxUnitIncreasePriceData;
    [SerializeField] CurrencyData[] _unitSellRewardDatas;
    [SerializeField] CurrencyData[] _whiteUnitPriceDatas;

    public ShopDataContainer CreateDataUseCase() => new(AddData, ScaleData, _maxUpgradeLevel, _maxUnitIncreasePriceData, _unitSellRewardDatas, _whiteUnitPriceDatas);

    public int ResetPrice;
    public UnitUpgradeShopData Clone()
    {
        var result = ScriptableObject.CreateInstance<UnitUpgradeShopData>();
        result.AddValue = AddValue;
        result.AddValuePriceData = AddValuePriceData.Cloen();
        result.UpScale = UpScale;
        result.UpScalePriceData = UpScalePriceData.Cloen();
        result.ResetPrice = ResetPrice;
        return result;
    }
}
