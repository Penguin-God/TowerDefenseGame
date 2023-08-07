using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTrakerSortByCombineable : UI_UnitTrackerParent
{
    readonly int MAX_UI_COUNT = 4;
    protected override void SortTrackers(UnitFlags flag) => gameObject.SetActive(true);
    void OnEnable()
    {
        Managers.Unit.OnUnitCountChange -= UpdateCombineableUnitFlags;
        Managers.Unit.OnUnitCountChange += UpdateCombineableUnitFlags;
        SortTrakers(); // 상대 진영볼 때 조합식이 바뀌는 경우가 있어서 OnEnable에서 돌림
    } 

    void SortTrakers()
    {
        var combineableUnitFalgs = Managers.Unit.CombineableUnitFlags;
        DestroyChilds();
        if (combineableUnitFalgs.Count() == 0) return;
        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
            Managers.UI.MakeSubItem<UI_UnitTracker>(transform).SetInfo(unitFlag);
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags // 정렬 후 뒤에서부터 MAX_UI_COUNT만큼 가져옴
            .Where(x => UnitFlags.NormalFlags.Contains(x))
            .OrderBy(x => x.ClassNumber)
            .ThenBy(x => x.ColorNumber)
            .Reverse()
            .Take(MAX_UI_COUNT)
            .Reverse();

    void UpdateCombineableUnitFlags(int count)
    {
        if (gameObject.activeSelf == false) return;
        SortTrakers();
    }

    void DestroyChilds()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    void OnDisable() => Managers.Unit.OnUnitCountChange -= UpdateCombineableUnitFlags;
}
