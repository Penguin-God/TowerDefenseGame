using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MultiBattleData
{
    public CurrencyManager CurrencyManager { get; private set; } = new CurrencyManager();
    public int CurrentMonsterCount;
    public int MaxMonsterCount;
    public bool MonsterOver() => CurrentMonsterCount >= MaxMonsterCount;

    public int CurrentUnitCount;
    public int MaxUnitCount;
    public bool UnitOver() => CurrentUnitCount >= MaxUnitCount;
}

public class ServerManager
{
    MultiData<UnitDamageInfoManager> _unitDamageManagers;
    public ServerManager(Dictionary<UnitFlags, UnitDamageInfo> damageInfos)
    {
        _unitDamageManagers = new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(new Dictionary<UnitFlags, UnitDamageInfo>(damageInfos)));
        _units = new MultiData<List<Multi_TeamSoldier>>(() => new List<Multi_TeamSoldier>());
        _battleData = new MultiData<MultiBattleData>(() => new MultiBattleData());
    }

    public UnitDamageInfo UnitDamageInfo(byte id, UnitFlags flag) => GetUnitDamageInfoManager(id).GetDamageInfo(flag);

    public UnitDamageInfoManager GetUnitDamageInfoManager(byte playerId) => _unitDamageManagers.GetData(playerId);

    readonly UnitDamageInfoChanger _unitDamageInfoChanger = new UnitDamageInfoChanger();
    public void AddUnitDamageValue(byte playerId, UnitFlags flag, int value, UnitStatType changeStatType)
        => _unitDamageInfoChanger.AddUnitDamageValue(GetUnitDamageInfoManager(playerId), flag, value, changeStatType);

    public void AddUnitDamageValue(byte playerId, Func<UnitFlags, bool> condition, int value, UnitStatType changeStatType)
        => _unitDamageInfoChanger.AddUnitDamageValue(GetUnitDamageInfoManager(playerId), condition, value, changeStatType);

    public void ScaleUnitDamageValue(byte playerId, Func<UnitFlags, bool> condition, float value, UnitStatType changeStatType)
        => _unitDamageInfoChanger.ScaleUnitDamageValue(GetUnitDamageInfoManager(playerId), condition, value, changeStatType);

    MultiData<List<Multi_TeamSoldier>> _units;
    public IReadOnlyList<Multi_TeamSoldier> GetUnits(byte playerId) => _units.GetData(playerId);
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.GetData(unit.UsingID).Add(unit);
        unit.OnDead += RemoveUnit;
    }
    void RemoveUnit(Multi_TeamSoldier unit) =>  _units.GetData(unit.UsingID).Remove(unit);

    MultiData<MultiBattleData> _battleData;
    public MultiBattleData GetBattleData(byte id) => _battleData.GetData(id);
}


public class UnitDamageInfoChanger
{
    public void AddUnitDamageValue(UnitDamageInfoManager unitDamageManagers, Func<UnitFlags, bool> condition, int value, UnitStatType changeStatType)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            AddUnitDamageValue(unitDamageManagers, flag, value, changeStatType);
    }

    public void AddUnitDamageValue(UnitDamageInfoManager unitDamageManagers, UnitFlags flag, int value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: unitDamageManagers.AddDamage(flag, value); break;
            case UnitStatType.BossDamage: unitDamageManagers.AddBossDamage(flag, value); break;
            case UnitStatType.All:
                unitDamageManagers.AddDamage(flag, value);
                unitDamageManagers.AddBossDamage(flag, value);
                break;
        }
    }

    public void ScaleUnitDamageValue(UnitDamageInfoManager unitDamageManagers, Func<UnitFlags, bool> condition, float value, UnitStatType changeStatType)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            ScaleUnitDamageValue(unitDamageManagers, flag, value, changeStatType);
    }

    public void ScaleUnitDamageValue(UnitDamageInfoManager unitDamageManagers, UnitFlags flag, float value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: unitDamageManagers.IncreaseDamageRate(flag, value); break;
            case UnitStatType.BossDamage: unitDamageManagers.IncreaseBossDamageRate(flag, value); break;
            case UnitStatType.All:
                unitDamageManagers.IncreaseDamageRate(flag, value);
                unitDamageManagers.IncreaseBossDamageRate(flag, value);
                break;
        }
    }
}