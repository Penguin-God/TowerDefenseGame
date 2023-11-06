using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UnitControllerManager
{
    readonly List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();
    public int CurrentUnitCount => _units.Count;
    public HashSet<UnitFlags> ExsitUnitFlags => new HashSet<UnitFlags>(_units.Select(x => x.UnitFlags));

    public void AddUnit(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        unit.OnDead += RemoveUnit;
    }

    void RemoveUnit(Multi_TeamSoldier unit)
    {
        _units.Remove(unit);
    }

    public void Clear()
    {
        _units.Clear();
    }

    public Multi_TeamSoldier FindUnit(UnitFlags flag) => FindUnit(x => x.UnitFlags == flag);
    public bool TryFindUnit(Func<Multi_TeamSoldier, bool> condition, out Multi_TeamSoldier result)
    {
        result = FindUnit(condition);
        return result != null;
    }
    public Multi_TeamSoldier FindUnit(Func<Multi_TeamSoldier, bool> condition) => FindUnits(condition).FirstOrDefault();
    IEnumerable<Multi_TeamSoldier> FindUnits(Func<Multi_TeamSoldier, bool> condition) => _units.Where(condition);
}