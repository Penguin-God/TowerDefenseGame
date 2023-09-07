using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamblerController
{
    public GamblerLevelSystem LevelSystem { get; private set; }
    readonly int ExpPrice;
    readonly IReadOnlyList<GambleData> _gambleDatas;
    public GamblerController(int expPrice, IReadOnlyList<GambleData> gambleDatas)
    {
        ExpPrice = expPrice;
        _gambleDatas = gambleDatas;
        LevelSystem = new GamblerLevelSystem(new LevelSystem(_gambleDatas.Select(x => x.NeedExpForLevelUp).ToArray()));
    }

    public event Action<UnitFlags> OnGamble;
    public void BuyExp(int amount)
    {
        if (Multi_GameManager.Instance.TryUseGold(ExpPrice))
            LevelSystem.AddExperience(amount);
    }

    public void GambleAndLevelUp()
    {
        Debug.Assert(LevelSystem.IsOverExp, "경험치가 부족한데 뽑기를 시도함");

        UnitFlags selectFlag = GambleUnit();
        Multi_SpawnManagers.NormalUnit.Spawn(selectFlag);
        LevelSystem.LevelUp();
    }

    UnitFlags GambleUnit()
    {
        var gachaTable = CreateUnitGachaData();
        int selectedIndex = new GachaMachine().SelectIndex(gachaTable.Select(x => x.Rate).ToArray());
        UnitFlags result = gachaTable.Select(x => x.GachaUnitFalgItems).ElementAt(selectedIndex).ToList().GetRandom();
        OnGamble?.Invoke(result);
        return result;
    }

    public IEnumerable<UnitGachaData> CreateUnitGachaData() => new GachaTableBuilder().CreateGachaTable(GetRates());
    int[] GetRates() => _gambleDatas[LevelSystem.Level - 1].GachaRates.ToArray();
}
