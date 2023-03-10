using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Spawners;

public class UnitManager
{
    UnitSpanwer _unitSpanwer;
    public UnitManager(UnitSpanwer unitSpanwer)
    {
        _unitSpanwer = unitSpanwer;
        _unitSpanwer.OnSpawn += UnitRegister;
    }

    List<Multi_TeamSoldier> _units = new List<Multi_TeamSoldier>();
    public int UnitCount => _units.Count;

    void UnitRegister(Multi_TeamSoldier unit)
    {
        _units.Add(unit);
        unit.OnDead += (deadUnit) => _units.Remove(deadUnit);
    }

    public bool TryCombine(UnitFlags flag)
    {
        if (new UnitCombineSystem().CheckCombineable(flag, (conditionFlag) => GetUnitCount(x => x == conditionFlag)))
        {
            Combine(flag);
            return true;
        }
        return false;
    }

    void Combine(UnitFlags flag)
    {
        foreach (var condition in Managers.Data.CombineConditionByUnitFalg[flag].NeedCountByFlag)
        {
            for (int i = 0; i < condition.Value; i++)
                GetUnit(flag).Dead();
        }
        
        _unitSpanwer.Spawn(flag);
    }

    public bool TryFindUnit(Func<UnitFlags, bool> condition, out Multi_TeamSoldier unit)
    {
        if(GetUnitCount(condition) == 0)
        {
            unit = null;
            return false;
        }
        else
        {
            unit = GetUnits(condition).FirstOrDefault();
            return true;
        }
    }

    public Multi_TeamSoldier GetUnit(UnitFlags flag) => GetUnits(x => x == flag).FirstOrDefault();
    public int GetUnitCount(Func<UnitFlags, bool> condition) => GetUnits(condition).Count();
    public IEnumerable<Multi_TeamSoldier> GetUnits(Func<UnitFlags, bool> condition) => _units.Where(x => condition(x.UnitFlags));
}
