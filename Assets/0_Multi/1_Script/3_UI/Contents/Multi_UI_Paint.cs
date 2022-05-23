using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Multi_UI_Paint : Multi_UI_Scene
{
    public event Action<UI_UnitTrackerData> OnPaintChanged = null;

    [SerializeField] GameObject _paintActiveButton;
    [SerializeField] UI_UnitTrackerSetter[] _unitTrackerSetters;
    [SerializeField] UI_UnitTracker[] _unitTrackers;
    protected override void Init()
    {
        base.Init();

        BindEvnet(_paintActiveButton, ChangePaintRootActive);
        SetterDataSetting();
        SetterInActivePaintSelect();
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive(PointerEventData data) => _paintRoot.SetActive(!_paintRoot.activeSelf);

    void SetterDataSetting() 
        => _unitTrackerSetters.ToList().ForEach(x => BindEvnet(x.gameObject, data => OnPaintChanged(x.UnitTrackerData)));
    void SetterInActivePaintSelect() 
        => _unitTrackerSetters.ToList().ForEach(x => BindEvnet(x.gameObject, data => _paintRoot.SetActive(false)));
}
