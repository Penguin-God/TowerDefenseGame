using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrackerSetterTrigger : Multi_UI_Base
{
    [SerializeField] UI_UnitTrackerData _unitTrackerData;
    [SerializeField] UI_UnitTrackerSetterBase _unitTrackserSetter;

    void Awake()
    {
        _unitTrackerData = CreateUnitTrackerData();
        AddEventToButton(SignalTheUnitTrackerSetter);
    }

    UI_UnitTrackerData CreateUnitTrackerData()
    {
        return new UI_UnitTrackerData(_unitTrackerData.UnitFlags,
            _unitTrackerData.Icon != null ? _unitTrackerData.Icon : GetOrInChildrenComponent<Image>().sprite,
            GetOrInChildrenComponent<Image>().color,
            _unitTrackerData.UnitClassName);
    }

    void AddEventToButton(UnityEngine.Events.UnityAction action)
        => GetOrInChildrenComponent<Button>().onClick.AddListener(action);

    void SignalTheUnitTrackerSetter()
        => _unitTrackserSetter.SettingUnitTrackers(_unitTrackerData);

    T GetOrInChildrenComponent<T>()
        => GetComponent<T>() != null ? GetComponent<T>() : GetComponentInChildren<T>();
}
