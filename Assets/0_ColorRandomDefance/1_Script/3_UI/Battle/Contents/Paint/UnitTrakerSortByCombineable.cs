using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTrakerSortByCombineable : MonoBehaviour
{
    readonly int MAX_UI_COUNT = 4;
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
        foreach (var unitFlag in SortUnitFlags(combineableUnitFalgs))
            Managers.UI.MakeSubItem<UI_UnitTracker>(transform).SetInfo(unitFlag);
    }

    IEnumerable<UnitFlags> SortUnitFlags(IEnumerable<UnitFlags> flags)
        => flags
            .Where(x => UnitFlags.NormalFlags.Contains(x))
            .OrderByDescending(x => x.ClassNumber)
            .ThenByDescending(x => x.ColorNumber)
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
