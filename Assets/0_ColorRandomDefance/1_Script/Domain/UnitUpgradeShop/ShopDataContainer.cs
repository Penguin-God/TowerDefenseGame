using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopDataContainer
{
    public readonly UnitUpgradeGoodsData AddData;
    public readonly UnitUpgradeGoodsData ScaleData;
    public readonly int UpgradeMaxLevel;
    public readonly CurrencyData MaxUnitIncreasePriceData;
    public readonly CurrencyData[] UnitSellRewardDatas;
    public readonly CurrencyData[] WhiteUnitPriceDatas;
    public ShopDataContainer(UnitUpgradeGoodsData addData, UnitUpgradeGoodsData scaleData, int upgradeMaxLevel, 
        CurrencyData unitIncreasePrice, IEnumerable<CurrencyData> unitSellRewardDatas, IEnumerable<CurrencyData> whiteUnitPriceDatas)
    {
        AddData = addData;
        ScaleData = scaleData;
        UpgradeMaxLevel = upgradeMaxLevel;
        MaxUnitIncreasePriceData = unitIncreasePrice;
        UnitSellRewardDatas = unitSellRewardDatas.ToArray();
        WhiteUnitPriceDatas = whiteUnitPriceDatas.ToArray();
    }

    public IEnumerable<CurrencyData> GetAllShopPriceDatas() =>
        WhiteUnitPriceDatas
        .Concat(UnitSellRewardDatas)
        .Concat(new CurrencyData[] { MaxUnitIncreasePriceData, AddData.Price, ScaleData.Price });

}
