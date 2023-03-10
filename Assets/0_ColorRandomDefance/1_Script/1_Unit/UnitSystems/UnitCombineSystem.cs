using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitCombineSystem
{
    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(IEnumerable<UnitFlags> currentUnits)
        => GetCombinableUnitFalgs((flag) => GetUnitCount(flag, currentUnits));

    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(Func<UnitFlags, int> getCount)
        => Managers.Data
            .CombineConditionByUnitFalg
            .Keys
            .Where(x => CheckCombineable(x, getCount));

    public bool CheckCombineable(UnitFlags targetFlag, Func<UnitFlags, int> getCount)
        => Managers.Data.CombineConditionByUnitFalg[targetFlag]
            .NeedCountByFlag
            .All(x => getCount(x.Key) >= x.Value);

    int GetUnitCount(UnitFlags flag, IEnumerable<UnitFlags> currentUnits) => currentUnits.Where(x => x == flag).Count();
}
