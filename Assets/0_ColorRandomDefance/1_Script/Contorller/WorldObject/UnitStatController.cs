using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}

public class UnitStatController
{
    readonly WorldUnitDamageManager _worldUnitDamageManager;
    readonly MultiData<UnitManager> _worldUnitManager;

    readonly Dictionary<UnitFlags, Vector2Int> _upgradeInfoByFlag = UnitFlags.AllFlags.ToDictionary(x => x, x => new Vector2Int());
    public void AddUnitUpgradeValue(UnitFlags flag, int value) => _upgradeInfoByFlag[flag] += new Vector2Int(value, 0);
    public void AddUnitUpgradeScale(UnitFlags flag, int value) => _upgradeInfoByFlag[flag] += new Vector2Int(0, value);
    public int GetUnitUpgradeValue(UnitFlags flag) => _upgradeInfoByFlag[flag].x;
    public int GetUnitUpgradeScale(UnitFlags flag) => _upgradeInfoByFlag[flag].y;

    public UnitStatController(WorldUnitDamageManager worldUnitDamageManager, MultiData<UnitManager> unitManager)
    {
        _worldUnitDamageManager = worldUnitDamageManager;
        _worldUnitManager = unitManager;
    }

    public UnitDamageInfo GetDamageInfo(UnitFlags flag, byte id) => _worldUnitDamageManager.GetUnitDamageInfo(flag, id);

    public void AddUnitDamageWithFlag(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
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

    public void ScaleAllUnitDamage(float value, UnitStatType changeStatType, byte id)
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
