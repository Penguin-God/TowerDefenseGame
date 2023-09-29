using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class WorldUnitDamageManager
{
    MultiData<UnitDamageInfoManager> _worldDamageData;
    UnitDamageInfoManager GetDamageManager(byte worldId) => _worldDamageData.GetData(worldId);
    public WorldUnitDamageManager(MultiData<UnitDamageInfoManager> worldDamageData) => _worldDamageData = worldDamageData;

    public UnitDamageInfo GetUnitDamageInfo(UnitFlags flag, byte worldId) => GetDamageManager(worldId).GetDamageInfo(flag);

    public void AddUnitDamageValue(Func<UnitFlags, bool> condition, int value, UnitStatType changeStatType, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            AddUnitDamageValue(flag, value, changeStatType, id);
    }

    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType, byte id)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: GetDamageManager(id).AddDamage(flag, value); break;
            case UnitStatType.BossDamage: GetDamageManager(id).AddBossDamage(flag, value); break;
            case UnitStatType.All:
                GetDamageManager(id).AddDamage(flag, value);
                GetDamageManager(id).AddBossDamage(flag, value);
                break;
        }
    }

    public void ScaleUnitDamageValue(Func<UnitFlags, bool> condition, float value, UnitStatType changeStatType, byte id)
    {
        foreach (var flag in UnitFlags.AllFlags.Where(condition))
            ScaleUnitDamageValue(flag, value, changeStatType, id);
    }

    public void ScaleUnitDamageValue(UnitFlags flag, float value, UnitStatType changeStatType, byte id)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: GetDamageManager(id).IncreaseDamageRate(flag, value); break;
            case UnitStatType.BossDamage: GetDamageManager(id).IncreaseBossDamageRate(flag, value); break;
            case UnitStatType.All:
                GetDamageManager(id).IncreaseDamageRate(flag, value);
                GetDamageManager(id).IncreaseBossDamageRate(flag, value);
                break;
        }
    }
}
