using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public enum UnitStatType
{
    Damage,
    BossDamage,
    All,
}

public class UnitStatController
{
    readonly Dictionary<UnitFlags, Vector2Int> _upgradeInfoByFlag = UnitFlags.AllFlags.ToDictionary(x => x, x => new Vector2Int());
    public void AddUnitUpgradeValue(UnitFlags flag, int value) => _upgradeInfoByFlag[flag] += new Vector2Int(value, 0);
    public void AddUnitUpgradeScale(UnitFlags flag, int value) => _upgradeInfoByFlag[flag] += new Vector2Int(0, value);
    public int GetUnitUpgradeValue(UnitFlags flag) => _upgradeInfoByFlag[flag].x;
    public int GetUnitUpgradeScale(UnitFlags flag) => _upgradeInfoByFlag[flag].y;

    MultiData<WorldUnitDamageManager> _worldUnitDamageManagers;
    readonly WorldUnitDamageManager _worldUnitDamageManager;
    WorldUnitManager _worldUnitManager;
    public UnitStatController(WorldUnitDamageManager worldUnitDamageManager, WorldUnitManager unitManager)
    {
        _worldUnitDamageManager = worldUnitDamageManager;
        _worldUnitManager = unitManager;
    }

    public UnitDamageInfo GetDamageInfo(UnitFlags flag, byte id) => _worldUnitDamageManager.GetUnitDamageInfo(flag, id);

    public void AddUnitDamage(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        _worldUnitDamageManager.AddUnitDamageValue(flag, value, changeStatType, id);
        AddUnitUpgradeValue(flag, value);
        UpdateCurrentUnitDamage(id);
    }

    public void AddUnitDamageWithColor(UnitColor color, int value, UnitStatType changeStatType, byte id)
        => AddUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);

    public void AddUnitDamageValue(Func<UnitFlags, bool> condition, int value, UnitStatType changeStatType, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            AddUnitDamage(flag, value, changeStatType, id);
    }

    public void ScaleUnitDamageWithColor(UnitColor color, float value, UnitStatType changeStatType, byte id)
        => ScaleUnitDamageValue(flag => SameColor(flag, color), value, changeStatType, id);

    public void ScaleAllUnitDamage(float value, UnitStatType changeStatType, byte id)
        => ScaleUnitDamageValue(_ => true, value, changeStatType, id);

    public void ScaleUnitDamageValue(Func<UnitFlags, bool> condition, float value, UnitStatType changeStatType, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
        {
            _worldUnitDamageManager.ScaleUnitDamageValue(flag, value, changeStatType, id);
            AddUnitUpgradeScale(flag, Mathf.RoundToInt(value * 100));
        }
        UpdateCurrentUnitDamage(id);
    }

    void UpdateCurrentUnitDamage(byte id)
    {
        foreach (var unit in _worldUnitManager.GetUnits(id))
            unit.UpdateDamageInfo(_worldUnitDamageManager.GetUnitDamageInfo(unit.UnitFlags, id));
    }


    bool SameColor(UnitFlags flag, UnitColor color) => flag.UnitColor == color;
}
