using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrakerSortByColor : UI_UnitTrackerParent
{
    protected override void SortTrackers(UnitFlags flag) => SortTrackers(flag.ColorNumber);

    void SortTrackers(int colorNumber)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<UI_UnitTracker>().SetInfo(new UnitFlags(colorNumber, i));
    }
}
