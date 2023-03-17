using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    public GameManager(Dictionary<UnitFlags, UnitDamageInfo> damageInfos)
        => _unitDamageManagers = new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(new Dictionary<UnitFlags, UnitDamageInfo>(damageInfos)));

    MultiData<UnitDamageInfoManager> _unitDamageManagers;
    public UnitDamageInfoManager GetUnitDamageInfoManager(byte playerId) => _unitDamageManagers.GetData(playerId);

    public void ChangeUnitDamageValue(byte playerId, UnitFlags flag, int value, UnitStatType changeStatType)
        => new UnitDamageInfoChanger().ChangeUnitDamageValue(GetUnitDamageInfoManager(playerId), flag, value, changeStatType);

    public void ScaleUnitDamageValue(byte playerId, UnitFlags flag, float value, UnitStatType changeStatType)
        => new UnitDamageInfoChanger().ScaleUnitDamageValue(GetUnitDamageInfoManager(playerId), flag, value, changeStatType);
}

public class UnitDamageInfoChanger
{
    public void ChangeUnitDamageValue(UnitDamageInfoManager unitDamageManagers, UnitFlags flag, int value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: unitDamageManagers.AddDamage(flag, value); break;
            case UnitStatType.BossDamage: unitDamageManagers.AddBossDamage(flag, value); break;
            case UnitStatType.All:
                unitDamageManagers.AddDamage(flag, value);
                unitDamageManagers.AddBossDamage(flag, value);
                break;
        }
    }

    public void ScaleUnitDamageValue(UnitDamageInfoManager unitDamageManagers, UnitFlags flag, float value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: unitDamageManagers.IncreaseDamageRate(flag, value); break;
            case UnitStatType.BossDamage: unitDamageManagers.IncreaseBossDamageRate(flag, value); break;
            case UnitStatType.All:
                unitDamageManagers.IncreaseDamageRate(flag, value);
                unitDamageManagers.IncreaseBossDamageRate(flag, value);
                break;
        }
    }
}