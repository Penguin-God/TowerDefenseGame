using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UI_SetterByClass : UI_UnitTrackerSetterBase
{
    public override void SettingUnitTrackers(UI_UnitTrackerData data)
    {
        base.SettingUnitTrackers(data);
        _unitTrackers.ToList().ForEach(x => x.UnitFlags = new UnitFlags(data.UnitFlags.ColorNumber, x.UnitFlags.ClassNumber));
        _unitTrackers.ToList().ForEach(x => x.Icon.sprite = data.Icon);
    }
}
