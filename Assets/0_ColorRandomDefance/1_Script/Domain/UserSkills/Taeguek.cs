using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaegeukStateChangeType
{
    NoChange,
    AddNewUnit,
    TrueToFalse,
    FalseToTrue,
}

public struct TaegeukState
{
    public TaegeukStateChangeType ChangeState;
    public bool IsActive;

    public TaegeukState(TaegeukStateChangeType changeType, bool isActive)
    {
        ChangeState = changeType;
        IsActive = isActive;
    }
}

public class TaegeukStateManager
{
    bool[] _currentTaegeukFlags = new bool[Enum.GetValues(typeof(UnitClass)).Length];

    public TaegeukState GetTaegeukState(UnitClass unitClass, HashSet<UnitFlags> exsitUnitFlags)
    {
        bool prevTaegeukFlag = _currentTaegeukFlags[(int)unitClass];
        bool newTaegeukFlag = new TaegeukConditionChecker().CheckTaegeuk(unitClass, exsitUnitFlags);
        _currentTaegeukFlags[(int)unitClass] = newTaegeukFlag;

        if (prevTaegeukFlag && newTaegeukFlag)
            return new TaegeukState(TaegeukStateChangeType.AddNewUnit, newTaegeukFlag);
        else if (prevTaegeukFlag && newTaegeukFlag == false)
            return new TaegeukState(TaegeukStateChangeType.TrueToFalse, newTaegeukFlag);
        else if (prevTaegeukFlag == false && newTaegeukFlag)
            return new TaegeukState(TaegeukStateChangeType.FalseToTrue, newTaegeukFlag);
        else
            return new TaegeukState(TaegeukStateChangeType.NoChange, newTaegeukFlag);
    }
}

public class TaegeukConditionChecker
{
    public bool CheckTaegeuk(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
        => ExistRedAndBlue(unitClass, existUnitFlags) && CountZeroTaegeukOther(unitClass, existUnitFlags);

    bool ExistRedAndBlue(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
        => existUnitFlags.Contains(new UnitFlags(UnitColor.Red, unitClass)) && existUnitFlags.Contains(new UnitFlags(UnitColor.Blue, unitClass));

    bool CountZeroTaegeukOther(UnitClass unitClass, HashSet<UnitFlags> existUnitFlags)
    {
        var otherColors = new UnitColor[] { UnitColor.Yellow, UnitColor.Green, UnitColor.Orange, UnitColor.Violet };
        return !otherColors.Any(color => existUnitFlags.Contains(new UnitFlags(color, unitClass)));
    }
}
