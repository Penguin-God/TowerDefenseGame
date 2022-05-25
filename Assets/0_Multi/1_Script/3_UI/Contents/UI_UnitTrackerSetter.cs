using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrackerSetter : Multi_UI_Base
{
    [SerializeField] UI_UnitTrackerData _unitTrackerData;
    public UI_UnitTrackerData UnitTrackerData => _unitTrackerData;

    [SerializeField] protected Button _trackerSettingButton;
    
    [SerializeField] UI_UnitTracker[] _targets;
    public IReadOnlyList<UI_UnitTracker> Targets => _targets;

    protected override void Init()
    {
        _unitTrackerData = new UI_UnitTrackerData(_unitTrackerData.UnitFlags, null, GetComponent<Image>().color);
    }

    public virtual void SetTracker()
    {

    }
}
