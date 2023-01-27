using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_UnitTrackerParent : UI_Base
{
    private UI_Paint _paint;
    protected UI_UnitTracker[] _unitTrackers;

    void Awake()
    {
        _paint = GetComponentInParent<UI_Paint>();
        _unitTrackers = GetComponentsInChildren<UI_UnitTracker>();
    }

    public void SettingUnitTrackers(UnitFlags flag)
    {
        gameObject.SetActive(true);

        UpdateCurrentTarckerParent();
        SetTrackersInfo(flag);
        CloseUnitWindowUI();
    }

    void UpdateCurrentTarckerParent()
    {
        if (_paint.CurrentUnitTracker != null && _paint.CurrentUnitTracker != gameObject)
        {
            _paint.CurrentUnitTracker.SetActive(false);
            _paint.CurrentUnitTracker = gameObject;
        }
    }

    void SetTrackersInfo(UnitFlags flag) => _unitTrackers.ToList().ForEach(x => x.SetInfo(flag));

    void CloseUnitWindowUI()
    {
        var popup = Managers.UI.PeekPopupUI();
        if (popup != null && popup.GetComponent<UI_UnitManagedWindow>() != null)
            Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
    }
}
