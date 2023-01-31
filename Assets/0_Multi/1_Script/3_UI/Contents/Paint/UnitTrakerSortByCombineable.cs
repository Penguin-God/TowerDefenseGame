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
        Multi_UnitManager.Instance.OnUnitFlagCountChanged -= (flag, count) => UpdateCombineableUnitFlags();
        Multi_UnitManager.Instance.OnUnitFlagCountChanged += (flag, count) => UpdateCombineableUnitFlags();
    }

    void SortTrakers()
    {
        var combineableUnitFalgs = Multi_UnitManager.Instance.CombineableUnitFlags;
        if (combineableUnitFalgs.Count() == 0) return;
        DestroyChilds();
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
        SortTrakers();
    }

    void DestroyChilds()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    void OnDisable() => DestroyChilds();
}
