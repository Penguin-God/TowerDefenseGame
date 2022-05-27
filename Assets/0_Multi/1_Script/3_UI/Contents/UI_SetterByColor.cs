using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_SetterByColor : UI_UnitTrackerSetterBase
{
    public override void SettingUnitTrackers(UI_UnitTrackerData data)
    {
        base.SettingUnitTrackers(data);
        _unitTrackers.ToList().ForEach(x => x.BackGround.color = data.BackGroundColor);
    }
}
