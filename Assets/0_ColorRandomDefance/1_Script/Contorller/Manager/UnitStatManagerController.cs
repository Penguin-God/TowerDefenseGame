using System.Collections;
using System.Collections.Generic;
using System;

public class UnitStatManagerController
{
    readonly WorldUnitDamageManager _worldUnitDamageManager;
    readonly UnitControllerManager _unitManager;

    void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.AddUnitDamageValue(flag, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, flag);
    }

    void AddUnitDamageValueWithColor(UnitColor color, int value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.AddUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, conditon);
    }

    void ScaleUnitDamageValueWithColor(UnitColor color, float value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.ScaleUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, conditon);
    }

    void ScaleAllUnitDamageValueWith(float value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.ScaleUnitDamageValue((x) => true, value, changeStatType, id);
        // UpdateCurrentUnitDamageInfo(id, (x) => true);
    }

    bool SameColor(UnitFlags flag, UnitColor color) => flag.UnitColor == color;
}
