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
    MultiData<UnitDamageInfoManager> _unitDamageManagers;
    public UnitDamageInfoManager GetInfoManager(byte id) => _unitDamageManagers.GetData(id);

    readonly WorldUnitManager _worldUnitManager;

    public UnitStatController(MultiData<UnitDamageInfoManager> unitDamageManagers, WorldUnitManager unitManager)
    {
        _unitDamageManagers = unitDamageManagers;
        _worldUnitManager = unitManager;
    }

    public UnitDamageInfo GetDamageInfo(UnitFlags flag, byte id) => GetInfoManager(id).GetDamageInfo(flag);

    public void UpgradeUnitDamage(Func<UnitFlags, bool> upgradeCondition, UnitDamageInfo info, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(upgradeCondition))
            GetInfoManager(id).UpgradeDamageInfo(flag, info);
        UpdateCurrentUnitDamage(id);
    }

    public void AddUnitDamage(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        GetInfoManager(id).AddDamage(flag, value, changeStatType);
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

    public void ScaleAllUnitDamage(float value, UnitStatType changeStatType, byte id) => ScaleUnitDamageValue(_ => true, value, changeStatType, id);

    public void ScaleUnitDamageValue(Func<UnitFlags, bool> condition, float value, UnitStatType changeStatType, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            GetInfoManager(id).ScaleDamage(flag, value, changeStatType);
        UpdateCurrentUnitDamage(id);
    }

    void UpdateCurrentUnitDamage(byte id)
    {
        foreach (var unit in _worldUnitManager.GetUnits(id))
            unit.UpdateDamageInfo(GetDamageInfo(unit.UnitFlags, id));
    }


    bool SameColor(UnitFlags flag, UnitColor color) => flag.UnitColor == color;
}
