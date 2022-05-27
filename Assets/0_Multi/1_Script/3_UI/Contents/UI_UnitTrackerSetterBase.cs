using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class UI_UnitTrackerSetterBase : Multi_UI_Base
{
    [SerializeField] protected UI_UnitTracker[] _unitTrackers;

    void Awake()
    {
        _unitTrackers = GetComponentsInChildren<UI_UnitTracker>();
    }

    public virtual void SettingUnitTrackers(UI_UnitTrackerData data)
    {
        gameObject.SetActive(true);
    }
}
