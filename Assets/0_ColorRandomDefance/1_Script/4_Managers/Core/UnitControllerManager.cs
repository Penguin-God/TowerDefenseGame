using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UnitControllerManager
{
    readonly List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();
    public int CurrentUnitCount => _units.Count;
    public HashSet<UnitFlags> ExsitUnitFlags => new HashSet<UnitFlags>(_units.Select(x => x.UnitFlags));

    public event Action<int> OnUnitCountChange = null;
    public event Action<UnitFlags, int> OnUnitCountChangeByFlag = null;
    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        NotifyChangeUnitCount(unit);
        unit.OnDead += RemoveUnit;
    }

    void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.Remove(unit);
        NotifyChangeUnitCount(unit);
    }

    void NotifyChangeUnitCount(Multi_TeamSoldier unit)
    {
        OnUnitCountChange?.Invoke(_units.Count);
        OnUnitCountChangeByFlag?.Invoke(unit.UnitFlags, FindUnits(x => x.UnitFlags == unit.UnitFlags).Count());
    }

    public void Inject(UnitCombineSystem combineSystem) => _combineSystem = combineSystem;

    public void Clear()
    {
        _units.Clear();
        OnUnitCountChange = null;
        OnUnitCountChangeByFlag = null;
    }

    UnitCombineSystem _combineSystem;
    public IEnumerable<UnitFlags> CombineableUnitFlags => _combineSystem.GetCombinableUnitFalgs(GetUnitCount);

    public Multi_TeamSoldier FindUnit(UnitFlags flag) => FindUnit(x => x.UnitFlags == flag);
    public bool TryFindUnit(Func<Multi_TeamSoldier, bool> condition, out Multi_TeamSoldier result)
    {
        result = FindUnit(condition);
        return result != null;
    }
    public Multi_TeamSoldier FindUnit(Func<Multi_TeamSoldier, bool> condition) => FindUnits(condition).FirstOrDefault();
    public int GetUnitCount(UnitFlags flag) => FindUnits(x => x.UnitFlags == flag).Count();
    public IEnumerable<Multi_TeamSoldier> FindUnits(Func<Multi_TeamSoldier, bool> condition) => _units.Where(condition);
}