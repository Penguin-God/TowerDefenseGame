using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppentStatusManager
{
    public event Action<int> OnUnitCountChanged = null;
    public event Action<UnitClass, int> OnUnitCountChangedByClass;
    public event Action<int> OnUnitMaxCountChanged = null;

    void NotifyOtherUnitCountChanged(byte count, UnitClass unitClass, byte classCoount)
    {
        OnUnitCountChanged?.Invoke(count);
        OnUnitCountChangedByClass?.Invoke(unitClass, classCoount);
    }

    void NotifyOtherUnitMaxCountChanged(byte maxCount) => OnUnitMaxCountChanged?.Invoke(maxCount);

    public void Init(OpponentStatusSynchronizer synchronizer)
    {
        synchronizer.OnOtherUnitCountChanged += NotifyOtherUnitCountChanged;
        synchronizer.OnOtherUnitMaxCountChanged += NotifyOtherUnitMaxCountChanged;
    }
}
