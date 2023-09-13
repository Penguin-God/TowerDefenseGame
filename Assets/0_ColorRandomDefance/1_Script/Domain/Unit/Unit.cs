using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Android.Types;

public enum UnitState
{
    Idle,
    Move,
    Attack,
    Die,
}

public struct UnitSpot
{
    public readonly byte WorldId;
    public readonly bool IsInDefenseWolrd;

    public UnitSpot(byte worldId, bool isInDefenseWolrd)
    {
        WorldId = worldId;
        IsInDefenseWolrd = isInDefenseWolrd;
    }
}

[Serializable]
public class Unit
{
    [SerializeField] UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    [SerializeField] UnitDamageInfo _damageInfo;
    public UnitDamageInfo DamageInfo => _damageInfo;
    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _damageInfo = newInfo;

    public UnitSpot UnitSpot { get; private set; }
    public Unit(UnitFlags flag, UnitDamageInfo damageInfo, UnitSpot unitSpot)
    {
        _unitFlags = flag;
        _damageInfo = damageInfo;
        UnitSpot = unitSpot;
    }

    UnitState _unitState;
    public void ChangeState(UnitState newState) => _unitState = newState;
    public void Dead() => ChangeState(UnitState.Die);
}
