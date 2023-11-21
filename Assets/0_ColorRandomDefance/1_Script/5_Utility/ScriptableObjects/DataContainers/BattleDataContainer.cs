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
    public int MaxMonsterCount;
    public UnitSummonData UnitSummonData;
    public int YellowKnightCombineGold;
    public CurrencyData[] UnitSellRewardDatas;
    public float WhiteUnitTime;
    public int StageMonsetSpawnCount;
    public float MonsterSpawnDelayTime;
    public float StageBreakTime;

    public BattleDataContainer Clone()
    {
        var result = ScriptableObject.CreateInstance<BattleDataContainer>();
        result.Gold = Gold;
        result.Food = Food;
        result.StageUpGold = StageUpGold;
        result.MaxUnit = MaxUnit;
        result.MaxMonsterCount = MaxMonsterCount;
        result.UnitSummonData = UnitSummonData;
        result.YellowKnightCombineGold = YellowKnightCombineGold;
        result.UnitSellRewardDatas = UnitSellRewardDatas.ToArray();
        result.WhiteUnitTime = WhiteUnitTime;
        result.StageMonsetSpawnCount = StageMonsetSpawnCount;
        result.MonsterSpawnDelayTime = MonsterSpawnDelayTime;
        result.StageBreakTime = StageBreakTime;
        return result;
    }
}
