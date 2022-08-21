using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_UnitTrackerSetterBase : Multi_UI_Base
{
    private Multi_UI_Paint _paint;
    protected UI_UnitTracker[] _unitTrackers;

    void Awake()
    {
        _paint = GetComponentInParent<Multi_UI_Paint>();
        _unitTrackers = GetComponentsInChildren<UI_UnitTracker>();
    }

    public virtual void SettingUnitTrackers(UI_UnitTrackerData data)
    {
        gameObject.SetActive(true);
        if (_paint.CurrentUnitTracker != null && _paint.CurrentUnitTracker != gameObject)
        {
            _paint.CurrentUnitTracker.SetActive(false);
            _paint.CurrentUnitTracker = gameObject;
        }

        _unitTrackers.ToList().ForEach(x => x.SetInfo(data));
        Multi_Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
    }
}
