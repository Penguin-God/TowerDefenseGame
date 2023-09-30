using System.Collections;
using System.Collections.Generic;

public class UnitStatController
{
    readonly WorldUnitDamageManager _worldUnitDamageManager;
    readonly MultiData<UnitManager> _worldUnitManager;

    public UnitStatController(WorldUnitDamageManager worldUnitDamageManager, MultiData<UnitManager> unitManager)
    {
        _worldUnitDamageManager = worldUnitDamageManager;
        _worldUnitManager = unitManager;
    }

    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.AddUnitDamageValue(flag, value, changeStatType, id);
        UpdateCurrentUnitDamage(id);
    }

    public void AddUnitDamageValueWithColor(UnitColor color, int value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.AddUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);
        UpdateCurrentUnitDamage(id);
    }

    public void ScaleUnitDamageValueWithColor(UnitColor color, float value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.ScaleUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);
        UpdateCurrentUnitDamage(id);
    }

    public void ScaleAllUnitDamageValueWith(float value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.ScaleUnitDamageValue((x) => true, value, changeStatType, id);
        UpdateCurrentUnitDamage(id);
    }

    bool SameColor(UnitFlags flag, UnitColor color) => flag.UnitColor == color;

    void UpdateCurrentUnitDamage(byte id)
    {
        foreach (var unit in _worldUnitManager.GetData(id).List)
            unit.UpdateDamageInfo(_worldUnitDamageManager.GetUnitDamageInfo(unit.UnitFlags, id));
    }
}
