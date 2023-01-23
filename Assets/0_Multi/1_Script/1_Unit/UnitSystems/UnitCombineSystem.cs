using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitCombineSystem
{
    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(IEnumerable<UnitFlags> currentUnits)
    {
        return Managers.Data
            .CombineConditionByUnitFalg
            .Keys
            .Where(x => CheckCombineable(x, (flag) => GetUnitCount(flag, currentUnits)));
    }

    public bool CheckCombineable(UnitFlags flag, Func<UnitFlags, int> getCount)
        => Managers.Data.CombineConditionByUnitFalg[flag]
            .NeedCountByFlag
            .All(x => getCount(x.Key) >= x.Value);

    int GetUnitCount(UnitFlags flag, IEnumerable<UnitFlags> currentUnits) => currentUnits.Where(x => x == flag).Count();
}
