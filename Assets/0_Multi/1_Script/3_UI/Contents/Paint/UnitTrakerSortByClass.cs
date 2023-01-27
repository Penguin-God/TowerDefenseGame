using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTrakerSortByClass : UI_UnitTrackerParent
{
    protected override void SortTrackers(UnitFlags flag) => SortTrakcers(flag.ClassNumber);

    private void SortTrakcers(int classNumber)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<UI_UnitTracker>().SetInfo(new UnitFlags(i, classNumber));
    }
}
