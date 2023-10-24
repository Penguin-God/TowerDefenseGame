using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct GambleData
{
    [SerializeField] int _needExpForLevelUp;
    public int NeedExpForLevelUp => _needExpForLevelUp;

    [SerializeField] int[] _gachaRates;
    public IReadOnlyList<int> GachaRates => _gachaRates;
}

public struct UnitGachaData
{
    public int Rate { get; private set; }
    public IEnumerable<UnitFlags> GachaUnitFalgItems { get; private set; }

    public UnitGachaData(int rate, IEnumerable<UnitFlags> unitFlags)
    {
        Rate = rate;
        GachaUnitFalgItems = unitFlags;
    }
}

public class GamblerController
{
    public GamblerLevelSystem LevelSystem { get; private set; }
    Multi_GameManager _game;
    readonly IReadOnlyList<GambleData> _gambleDatas;
    readonly Multi_NormalUnitSpawner _unitSpawner;
    public GamblerController(IReadOnlyList<GambleData> gambleDatas, Multi_GameManager game, Multi_NormalUnitSpawner unitSpawner)
    {
        _gambleDatas = gambleDatas;
        LevelSystem = new GamblerLevelSystem(new LevelSystem(_gambleDatas.Select(x => x.NeedExpForLevelUp).ToArray()));
        _game = game;
        _unitSpawner = unitSpawner;
    }

    public event Action<UnitFlags> OnGamble;

    public void BuyExp(int price, int amount)
    {
        if (_game.TryUseGold(price))
            AddExp(amount);
    }

    public void AddExp(int amount) => LevelSystem.AddExperience(amount);

    public void GambleAndLevelUp()
    {
        Debug.Assert(LevelSystem.IsOverExp, "경험치가 부족한데 뽑기를 시도함");

        UnitFlags selectFlag = GambleUnit();
        _unitSpawner.Spawn(selectFlag);
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
