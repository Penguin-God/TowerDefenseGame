using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UnitsData
{
    public int CurrentUnitCount { get; private set; }
    public int MaxUnitCount;
    public List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();
    public IReadOnlyList<Multi_TeamSoldier> Units => _units;
    public bool UnitOver() => CurrentUnitCount >= MaxUnitCount;

    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        CurrentUnitCount = _units.Count;
    }

    public void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.Remove(unit);
        CurrentUnitCount = _units.Count;
    }
}

public class MultiServiceLocator : Singleton<MultiServiceLocator>
{
    IDictionary<Type, object> _services = new Dictionary<Type, object>();

    public void RegisterService<T>() where T : new() => RegisterService(() => new T());
    void RegisterService<T>(Func<T> service) => _services[typeof(T)] = new MultiData<T>(service);

    public T GetService<T>(byte id) => ((MultiData<T>)_services[typeof(T)]).GetData(id);
}

public class ServerManager
{
    MultiData<UnitDamageInfoManager> _unitDamageManagers;
    public ServerManager(Dictionary<UnitFlags, UnitDamageInfo> damageInfos)
    {
        _unitDamageManagers = new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(new Dictionary<UnitFlags, UnitDamageInfo>(damageInfos)));
        _unitsData = new MultiData<UnitsData>(() => new UnitsData());
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

    public IReadOnlyList<Multi_TeamSoldier> GetUnits(byte playerId) => _unitsData.GetData(playerId).Units;
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _unitsData.GetData(unit.UsingID).AddUnit(unit);
        unit.OnDead += _unitsData.GetData(unit.UsingID).RemoveUnit;
    }

    MultiData<UnitsData> _unitsData;
    public UnitsData GetUnitstData(byte id) => _unitsData.GetData(id);
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
