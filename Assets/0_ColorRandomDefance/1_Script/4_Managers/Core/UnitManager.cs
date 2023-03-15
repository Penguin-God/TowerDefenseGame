using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class UnitManager
{
    List<Unit> _units = new List<Unit>();
    public int UnitCount => _units.Count;

    public event Action<Unit> OnRemoveUnit;

    public void RegisterUnit(Unit unit)
    {
        _units.Add(unit);
        unit.OnDead += RemoveUnit;
    }

    void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
        OnRemoveUnit?.Invoke(unit);
    }

    public bool CheckCombinealbe(UnitFlags flag) => new UnitCombineSystem(null).CheckCombineable(flag, (conditionFlag) => GetUnitCount(x => x == conditionFlag));

    public Unit GetUnit(UnitFlags flag) => GetUnits(x => x == flag).FirstOrDefault();
    public int GetUnitCount(Func<UnitFlags, bool> condition) => GetUnits(condition).Count();
    public IEnumerable<Unit> GetUnits(Func<UnitFlags, bool> condition) => _units.Where(x => condition(x.UnitFlags));
}
