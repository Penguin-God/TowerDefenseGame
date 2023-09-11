using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public Unit(UnitFlags flag, UnitDamageInfo damageInfo)
    {
        _unitFlags = flag;
        _damageInfo = damageInfo;
    }

    UnitState _unitState;
    public void ChangeState(UnitState newState) => _unitState = newState;
    public void Dead() => ChangeState(UnitState.Die);
}
