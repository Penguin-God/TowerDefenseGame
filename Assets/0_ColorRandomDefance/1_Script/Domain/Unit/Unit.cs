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

[Serializable]
public class Unit
{
    [SerializeField] UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    [SerializeField] UnitDamageInfo _damageInfo;
    public UnitDamageInfo DamageInfo => _damageInfo;
    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _damageInfo = newInfo;

    public ObjectSpot UnitSpot { get; private set; }
    public Unit(UnitFlags flag, UnitDamageInfo damageInfo, ObjectSpot unitSpot)
    {
        _unitFlags = flag;
        _damageInfo = damageInfo;
        UnitSpot = unitSpot;
    }
}
