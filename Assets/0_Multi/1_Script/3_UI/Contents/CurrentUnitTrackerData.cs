using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurrentUnitTrackerData : MonoBehaviour
{
    private static CurrentUnitTrackerData instance;
    public static CurrentUnitTrackerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CurrentUnitTrackerData>();
                if (instance == null) instance = new GameObject("CurrentUnitTrackerData").AddComponent<CurrentUnitTrackerData>();
            }
            return instance;
        }
    }

    public event Action<UnitFlags> OnUnitFlagChange;
    [SerializeField] UnitFlags _currentUnitFlags;

    public void SetFlag(UnitFlags newFlag)
    {
        if (_currentUnitFlags == newFlag) return;

        _currentUnitFlags = newFlag;
        OnUnitFlagChange?.Invoke(newFlag);
    }
}
