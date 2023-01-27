using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrackerSetterTrigger : UI_Base
{
    [SerializeField] UI_UnitTrackerParent _unitTrackerParent;
    [SerializeField] UnitFlags _flag;

    void Awake()
    {
        AddEventToButton(SignalTheUnitTrackerSetter);
        AddEventToButton(() => transform.parent.gameObject.SetActive(false));
    }

    void AddEventToButton(UnityEngine.Events.UnityAction action)
        => GetOrInChildrenComponent<Button>().onClick.AddListener(action);

    void SignalTheUnitTrackerSetter()
    {
        Managers.Sound.PlayEffect(EffectSoundType.SelectColor);
        _unitTrackerParent.SettingUnitTrackers(_flag); // UnitFlags만 줘야됨
    }

    T GetOrInChildrenComponent<T>()
        => GetComponent<T>() != null ? GetComponent<T>() : GetComponentInChildren<T>();
}
