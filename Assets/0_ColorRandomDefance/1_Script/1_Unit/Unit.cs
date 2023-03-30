using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
}
