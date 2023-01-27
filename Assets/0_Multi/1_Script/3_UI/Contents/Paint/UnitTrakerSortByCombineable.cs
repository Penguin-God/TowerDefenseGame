using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTrakerSortByCombineable : UI_UnitTrackerParent
{
    readonly int MAX_UI_COUNT = 4;
    protected override void SortTrackers(UnitFlags flag) => SortTrakers();

    protected override void Init()
    {
        base.Init();
        Multi_UnitManager.Instance.OnUnitCountChanged -= (count) => UpdateCombineableUnitFlags();
        Multi_UnitManager.Instance.OnUnitCountChanged += (count) => UpdateCombineableUnitFlags();
    }

    void SortTrakers()
    {
        var combineableUnitFalgs = new UnitCombineSystem().GetCombinableUnitFalgs(Multi_UnitManager.Instance.Master.GetUnits(Multi_Data.instance.Id).Select(x => x.UnitFlags));
        if (combineableUnitFalgs.Count() == 0) return;
        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
            Managers.UI.MakeSubItem<UI_UnitTracker>(transform).SetInfo(unitFlag);
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .OrderBy(x => x.ClassNumber)
            .ThenBy(x => x.ColorNumber)
            .Reverse()
            .Take(MAX_UI_COUNT)
            .Reverse();

    void UpdateCombineableUnitFlags()
    {
        if (gameObject.activeSelf == false) return;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        SortTrakers();
    }

    void OnDisable()
    {
        foreach (Transform item in transform)
            Destroy(item.gameObject);
    }
}
