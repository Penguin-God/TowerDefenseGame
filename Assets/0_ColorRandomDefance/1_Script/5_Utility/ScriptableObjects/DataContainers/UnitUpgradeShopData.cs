using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/UnitUpgradeShop")]
public class UnitUpgradeShopData : ScriptableObject
{
    [SerializeField] int _addValue;
    [SerializeField]  CurrencyData _addValuePriceData;
    [SerializeField]  UnitUpgradeGoodsData AddData => new (UnitDamageInfo.CreateDamageInfo(_addValue), UnitUpgradeType.Value, _addValuePriceData);

    [SerializeField]  float _upScale;
    [SerializeField]  CurrencyData _upScalePriceData;
    public UnitUpgradeGoodsData ScaleData => new (UnitDamageInfo.CreateRateInfo(_upScale), UnitUpgradeType.Scale, _upScalePriceData);
    [SerializeField] int _maxUpgradeLevel;

    [SerializeField] CurrencyData _maxUnitIncreasePriceData;
    [SerializeField] CurrencyData[] _whiteUnitPriceDatas;

    public ShopDataContainer CreateDataUseCase() => new(AddData, ScaleData, _maxUpgradeLevel, _maxUnitIncreasePriceData, _whiteUnitPriceDatas);
}
