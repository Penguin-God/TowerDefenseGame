using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTrakerSortByCombineable : UI_UnitTrackerParent
{
    readonly int MAX_UI_COUNT = 4;
    protected override void SortTrackers(UnitFlags flag)
    {
        var combineableUnitFalgs = new UnitCombineSystem().GetCombinableUnitFalgs(Multi_UnitManager.Instance.Master.GetUnits(Multi_Data.instance.Id).Select(x => x.UnitFlags));
        if (combineableUnitFalgs.Count() == 0) return;
        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs).Take(MAX_UI_COUNT))
            Managers.UI.MakeSubItem<UI_UnitTracker>(transform).SetInfo(unitFlag);
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .OrderBy(x => x.ClassNumber)
            .ThenBy(x => x.ColorNumber);

    void OnDisable()
    {
        foreach (Transform item in transform)
            Destroy(item.gameObject);
    }
}
