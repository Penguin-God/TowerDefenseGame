using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Multi_UnitManager : Singleton<Multi_UnitManager>
{
    [SerializeField] List<Multi_TeamSoldier> _units;
    public int CurrentUnitCount => _units.Count;
    public HashSet<UnitFlags> ExsitUnitFlags => new HashSet<UnitFlags>(_units.Select(x => x.UnitFlags));

    public Action<int> OnUnitCountChange = null;
    public Action<UnitFlags, int> OnUnitCountChangeByFlag = null;
    public Action<UnitClass, int> OnUnitCountChangeByClass = null;
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
        OnUnitCountChangeByClass?.Invoke(unit.UnitClass, FindUnits(x => x.UnitClass == unit.UnitClass).Count());
    }

    public override void Init()
    {
        base.Init();
        _combineSystem = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg);
    }

    UnitCombineSystem _combineSystem;
    public event Action<UnitFlags> OnCombine = null;
    public event Action OnFailedCombine = null;
    public IEnumerable<UnitFlags> CombineableUnitFlags => _combineSystem.GetCombinableUnitFalgs(GetUnitCount);

    public bool TryCombine(UnitFlags flag)
    {
        if (_combineSystem.CheckCombineable(flag, GetUnitCount))
        {
            Combine(flag);
            OnCombine?.Invoke(flag);
            return true;
        }
        else
        {
            OnFailedCombine?.Invoke();
            return false;
        }
    }

    void Combine(UnitFlags flag)
    {
        foreach (var needFlag in _combineSystem.GetNeedFlags(flag))
            FindUnit(needFlag).Dead();

        Multi_SpawnManagers.NormalUnit.Spawn(flag);
    }


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