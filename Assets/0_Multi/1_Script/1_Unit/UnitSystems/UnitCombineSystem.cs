using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitCombineSystem
{
    public IEnumerable<UnitFlags> GetCombinableUnitFalgs(IEnumerable<UnitFlags> currentUnits)
    {
        return Managers.Data
            .CombineConditionByUnitFalg
            .Keys
            .Where(x => CheckCombineable(x, currentUnits));
    }

    bool CheckCombineable(UnitFlags flag, IEnumerable<UnitFlags> currentUnits)
        => Managers.Data.CombineConditionByUnitFalg[flag]
            .NeedCountByFlag
            .All(x => GetUnitCount(x.Key, currentUnits) >= x.Value);

    int GetUnitCount(UnitFlags flag, IEnumerable<UnitFlags> currentUnits) => currentUnits.Where(x => x == flag).Count();
}
