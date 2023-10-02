using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitTrackerParent : UI_Base
{
    private UI_Paint _paint;
    
    void Awake()
    {
        _paint = GetComponentInParent<UI_Paint>();
    }

    UnitStatController _unitStatController;
    public void DependencyInject(UnitStatController unitStatController) => _unitStatController = unitStatController;
    protected override void Init() => new UnitTooltipController(_unitStatController).SetMouseOverAction(GetComponentsInChildren<UI_UnitTracker>());

    public void SettingUnitTrackers(UnitFlags flag)
    {
        gameObject.SetActive(true);

        UpdateCurrentTarckerParent();
        SortTrackers(flag);
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

    protected virtual void SortTrackers(UnitFlags flag) { }

    void CloseUnitWindowUI()
    {
        var popup = Managers.UI.PeekPopupUI();
        if (popup != null && popup.GetComponent<UI_UnitManagedWindow>() != null)
            Managers.UI.ClosePopupUI();
    }
}
