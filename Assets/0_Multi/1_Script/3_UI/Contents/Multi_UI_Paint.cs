using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public enum OrderType
{
    Color,
    Class,
}

public class Multi_UI_Paint : Multi_UI_Scene
{
    public event Action<UI_UnitTrackerData> OnPaintChanged = null;

    [SerializeField] GameObject _paintActiveButton;
    [SerializeField] GameObject _classButton;
    [SerializeField] GameObject _currentUnitTracker;
    [SerializeField] UI_UnitManagedWindow unitManagedWindow;
    [SerializeField] UI_UnitTrackerSetter[] _unitTrackerSetters;

    [SerializeField] GameObject _unitByColor;
    [SerializeField] GameObject _unitByClass;

    [SerializeField] UI_UnitTracker[] _unitTrackersByColor;
    public IReadOnlyList<UI_UnitTracker> UnitTrackersByColor => _unitTrackersByColor;

    [SerializeField] UI_UnitTracker[] _unitTrackersByClass;
    public IReadOnlyList<UI_UnitTracker> UnitTrackersByClass => _unitTrackersByClass;

    protected override void Init()
    {
        base.Init();

        BindEvnet(_paintActiveButton, ChangePaintRootActive);
        BindEvnet(_classButton, data => ChangeOrderType(OrderType.Class));

        SetterDataSetting();
        SetterInActivePaintSelect();
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive(PointerEventData data) => _paintRoot.SetActive(!_paintRoot.activeSelf);

    void SetterDataSetting() 
        => _unitTrackerSetters.ToList().ForEach(x => BindEvnet(x.gameObject, data => OnPaintChanged(x.UnitTrackerData)));
    void SetterInActivePaintSelect() 
        => _unitTrackerSetters.ToList().ForEach(x => BindEvnet(x.gameObject, data => _paintRoot.SetActive(false)));

    public void ShowUnitManagedWindow(UnitFlags flags) => unitManagedWindow.Show(flags);

    public void ChangeOrderType(OrderType type)
    {
        switch (type)
        {
            case OrderType.Color:
                _unitByColor.gameObject.SetActive(true);
                _unitByClass.gameObject.SetActive(false);
                _unitTrackersByColor.ToList().ForEach(x => x.GetComponent<UI_SetterByClass>().enabled = false);
                break;
            case OrderType.Class:
                _unitByColor.gameObject.SetActive(false);
                _unitByClass.gameObject.SetActive(true);
                _unitTrackersByColor.ToList().ForEach(x => x.GetComponent<UI_SetterByClass>().enabled = true);
                break;
        }
    }
}
