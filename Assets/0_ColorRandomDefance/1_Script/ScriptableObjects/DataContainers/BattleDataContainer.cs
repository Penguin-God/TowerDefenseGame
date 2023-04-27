using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/BattleData")]
public class BattleDataContainer : ScriptableObject
{
    public int Gold;
    public int Food;
    public int StageUpGold;
    public int MaxUnit;
    public int MaxEnemy;
    public int UnitSummonPrice;
    public UnitColor UnitSummonMaxColor;
    public int YellowKnightCombineGold;
    public CurrencyData MaxUnitIncreasePriceData;
    public CurrencyData[] WhiteUnitPriceDatas;
    public CurrencyData[] UnitSellRewardDatas;
}
