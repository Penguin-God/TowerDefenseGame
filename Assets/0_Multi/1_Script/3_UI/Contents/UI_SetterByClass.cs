using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UI_SetterByClass : UI_UnitTrackerSetter
{
    protected override void Init()
    {
        _trackerSettingButton = GetComponent<Button>();
        _trackerSettingButton.onClick.AddListener(SetTracker);
    }

    public override void SetTracker()
        => Targets.ToList().ForEach(x => x.SetInfoByClass(UnitTrackerData));
}
