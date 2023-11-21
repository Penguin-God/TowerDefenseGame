using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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

    void UpdateCurrentUnitDamage(byte id)
    {
        foreach (var unit in _worldUnitManager.GetUnits(id))
            unit.UpdateDamageInfo(GetDamageInfo(unit.UnitFlags, id));
    }
}
