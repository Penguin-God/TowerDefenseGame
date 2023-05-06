using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppentStatusManager
{
    public event Action<int> OnUnitCountChanged = null;
    public event Action<int> OnUnitMaxCountChanged = null;

    void NotifyOtherUnitCountChanged(byte count, UnitFlags flag, byte flagCount)
    {
        OnUnitCountChanged?.Invoke(count);
    }

    void NotifyOtherUnitMaxCountChanged(byte maxCount)
    {
        Debug.Log(maxCount);
        OnUnitMaxCountChanged?.Invoke(maxCount);
    }

    public void Init(OpponentStatusSynchronizer synchronizer)
    {
        synchronizer.OnOtherUnitCountChanged += NotifyOtherUnitCountChanged;
        synchronizer.OnOtherUnitMaxCountChanged += NotifyOtherUnitMaxCountChanged;
    }
}
