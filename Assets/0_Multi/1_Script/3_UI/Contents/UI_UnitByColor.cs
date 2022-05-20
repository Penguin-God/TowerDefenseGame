using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_UnitByColor : Multi_UI_Base
{
    [SerializeField] UI_UnitTracker[] unitTrackers;

    void Awake()
    {
        unitTrackers = GetComponentsInChildren<UI_UnitTracker>();
    }

    protected override void Init()
    {
        GetComponentInParent<Multi_UI_Paint>().OnPaintChanged += SetInfos;
    }

    void SetInfos(UI_UnitTrackerData data)
    {
        foreach (var tracker in unitTrackers)
        {
            tracker.SetInfo(data);
        }
    }
}
