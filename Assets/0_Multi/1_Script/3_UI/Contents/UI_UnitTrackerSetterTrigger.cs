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
        CreateUnitTrackerData();
        AddEventToButton(SignalTheUnitTrackerSetter);
    }

    void CreateUnitTrackerData()
    {
        _unitTrackerData = new UI_UnitTrackerData(_unitTrackerData.UnitFlags,
            GetComponent<Image>().sprite, GetComponent<Image>().color);
    }

    void AddEventToButton(UnityEngine.Events.UnityAction action)
    {
        Button button = GetComponent<Button>() ? GetComponent<Button>() : GetComponentInChildren<Button>();
        button.onClick.AddListener(action);
    }

    void SignalTheUnitTrackerSetter()
        => _unitTrackserSetter.SettingUnitTrackers(_unitTrackerData);
}
