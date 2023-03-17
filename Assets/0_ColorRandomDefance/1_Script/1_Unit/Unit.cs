﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit
{
    readonly UnitFlags _unitFlags;
    public UnitFlags UnitFlags => _unitFlags;

    UnitDamageInfo _damageInfo;
    public UnitDamageInfo DamageInfo => _damageInfo;
    public void UpdateDamageInfo(UnitDamageInfo newInfo) => _damageInfo = newInfo;

    public Unit(UnitFlags flag, UnitDamageInfo damageInfo)
    {
        _unitFlags = flag;
        _damageInfo = damageInfo;
    }
}
