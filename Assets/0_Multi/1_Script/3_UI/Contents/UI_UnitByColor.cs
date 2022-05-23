using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class UI_UnitByColor : Multi_UI_Base
{
    [SerializeField] UI_UnitTracker[] unitTrackers;

    void Awake()
    {
        unitTrackers = GetComponentsInChildren<UI_UnitTracker>();
    }

    protected override void Init()
    {
        GetComponentInParent<Multi_UI_Paint>().OnPaintChanged += SetInfosByColor;
    }

    void SetInfosByColor(UI_UnitTrackerData data) => unitTrackers.ToList().ForEach(x => x.SetInfoByColor(data));
    void SetInfosByClass(UI_UnitTrackerData data) => unitTrackers.ToList().ForEach(x => x.SetInfoByClass(data));
}
