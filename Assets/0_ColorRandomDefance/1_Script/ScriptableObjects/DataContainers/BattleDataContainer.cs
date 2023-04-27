using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/BattleData")]
public class BattleDataContainer : ScriptableObject
{
    public int Gold;
    public int Food;
    public int StageUpGold;
    public int MaxUnit;
    public int MaxEnemy;
    public UnitSummonData UnitSummonData;
    public int YellowKnightCombineGold;
    public CurrencyData MaxUnitIncreasePriceData;
    public CurrencyData[] UnitSellRewardDatas;
    public CurrencyData[] WhiteUnitPriceDatas;

    public BattleDataContainer Clone()
    {
        var result = ScriptableObject.CreateInstance<BattleDataContainer>();
        result.Gold = Gold;
        result.Food = Food;
        result.StageUpGold = StageUpGold;
        result.MaxUnit = MaxUnit;
        result.MaxEnemy = MaxEnemy;
        result.UnitSummonData = UnitSummonData;
        result.MaxUnitIncreasePriceData = MaxUnitIncreasePriceData.Cloen();
        result.UnitSellRewardDatas = UnitSellRewardDatas.ToArray();
        result.WhiteUnitPriceDatas = WhiteUnitPriceDatas.ToArray();
        return result;
    }
}
