using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Multi_UI_Paint : Multi_UI_Scene
{
    public event Action<UI_UnitTrackerData> OnPaintChanged = null;

    [SerializeField] UI_UnitTrackerSetter[] _unitTrackerSetters;
    [SerializeField] GameObject _paintActiveButton;
    protected override void Init()
    {
        base.Init();
        SettingSetter();
        BindEvnet(_paintActiveButton, ChangePaintRootActive);
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive(PointerEventData data) => _paintRoot.SetActive(!_paintRoot.activeSelf);

    void SettingSetter()
    {
        foreach (var setter in GetComponentsInChildren<UI_UnitTrackerSetter>())
            setter.GetComponentInChildren<Button>().onClick.AddListener(() => OnPaintChanged(setter.UnitTrackerData));
    }
}
