using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldUnitDamageManager
{
    MultiData<UnitDamageInfoManager> _worldDamageData;
    readonly UnitDamageInfoManager _damageInfoManager;
    UnitDamageInfoManager GetDamageManager(byte worldId) => _worldDamageData.GetData(worldId);
    public WorldUnitDamageManager(MultiData<UnitDamageInfoManager> worldDamageData) => _worldDamageData = worldDamageData;

    public UnitDamageInfo GetUnitDamageInfo(UnitFlags flag, byte worldId) => GetDamageManager(worldId).GetDamageInfo(flag);

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

    readonly Dictionary<UnitFlags, UnitDamageInfo> _upgradeAmountInfoByFlag = UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo());
    public UnitDamageInfo GetUnitUpgradeInfo(UnitFlags flag) => _upgradeAmountInfoByFlag[flag];

    public void AddUnitDamageValue(UnitFlags flag, int value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: AddDamage(flag, value); break;
            case UnitStatType.BossDamage: AddBossDamage(flag, value); break;
            case UnitStatType.All:
                AddDamage(flag, value);
                AddBossDamage(flag, value);
                break;
        }
    }

    void AddDamage(UnitFlags flag, int value)
    {
        _damageInfoManager.AddDamage(flag, value);
        _upgradeAmountInfoByFlag[flag] = _upgradeAmountInfoByFlag[flag].AddDamage(value);
    }

    void AddBossDamage(UnitFlags flag, int value)
    {
        _damageInfoManager.AddBossDamage(flag, value);
        _upgradeAmountInfoByFlag[flag] = _upgradeAmountInfoByFlag[flag].AddBossDamage(value);
    }

    public void ScaleUnitDamageValue(UnitFlags flag, float value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: ScaleDamage(flag, value); break;
            case UnitStatType.BossDamage: ScaleBossDamage(flag, value); break;
            case UnitStatType.All:
                ScaleDamage(flag, value);
                ScaleBossDamage(flag, value);
                break;
        }
    }

    void ScaleDamage(UnitFlags flag, float value)
    {
        _damageInfoManager.IncreaseDamageRate(flag, value);
        _upgradeAmountInfoByFlag[flag] = _upgradeAmountInfoByFlag[flag].IncreaseDamageRate(value);
    }

    void ScaleBossDamage(UnitFlags flag, float value)
    {
        _damageInfoManager.IncreaseBossDamageRate(flag, value);
        _upgradeAmountInfoByFlag[flag] = _upgradeAmountInfoByFlag[flag].IncreaseBossDamageRate(value);
    }
}
