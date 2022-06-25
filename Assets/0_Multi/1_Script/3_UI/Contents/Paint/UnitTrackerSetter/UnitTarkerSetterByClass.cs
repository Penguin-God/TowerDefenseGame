using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitTarkerSetterByClass : UI_UnitTrackerSetterBase
{
    public override void SettingUnitTrackers(UI_UnitTrackerData data)
    {
        base.SettingUnitTrackers(data);

        data = new UI_UnitTrackerData();
        _unitTrackers.ToList().ForEach(x => x.SetInfo(data));
    }
}
