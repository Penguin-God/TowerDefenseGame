using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrackerSetterTrigger : UI_Base
{
    [SerializeField] UI_UnitTrackerData _unitTrackerData;
    [SerializeField] UI_UnitTrackerParent _unitTrackserSetter;
    [SerializeField] UnitFlags flag;

    void Awake()
    {
        _unitTrackerData = CreateUnitTrackerData();
        AddEventToButton(SignalTheUnitTrackerSetter);
        AddEventToButton(() => transform.parent.gameObject.SetActive(false));
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
    {
        Managers.Sound.PlayEffect(EffectSoundType.SelectColor);
        _unitTrackserSetter.SettingUnitTrackers(_unitTrackerData); // UnitFlags만 줘야됨
    }

    T GetOrInChildrenComponent<T>()
        => GetComponent<T>() != null ? GetComponent<T>() : GetComponentInChildren<T>();
}
