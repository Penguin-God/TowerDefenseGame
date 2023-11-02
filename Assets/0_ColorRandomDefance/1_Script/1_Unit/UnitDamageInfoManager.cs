using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct UnitDamageInfo
{
    [SerializeField] int _baseDamage;
    public int BaseDamage => _baseDamage;
    [SerializeField] int _baseBossDamage;
    public int BaseBossDamage => _baseBossDamage;
    [SerializeField] float _damageRate;
    public float DamageRate => _damageRate;
    [SerializeField] float _bossDamageRate;
    public float BossDamageRate => _bossDamageRate;

    public UnitDamageInfo(int dam, int bossDam)
    {
        _baseDamage = dam;
        _baseBossDamage = bossDam;

        const float DEFAULT_RATE = 1f;
        _damageRate = DEFAULT_RATE;
        _bossDamageRate = DEFAULT_RATE;
    }

    public int ApplyDamage => Mathf.RoundToInt(_baseDamage * _damageRate);
    public int ApplyBossDamage => Mathf.RoundToInt(_baseBossDamage * _bossDamageRate);

    public UnitDamageInfo(int dam, int bossDam, float damRate, float bossDamRate)
    {
        _baseDamage = dam;
        _baseBossDamage = bossDam;
        _damageRate = damRate;
        _bossDamageRate = bossDamRate;
    }

    public UnitDamageInfo AddDamage(int addValue) => new UnitDamageInfo(_baseDamage + addValue, _baseBossDamage, _damageRate, _bossDamageRate);
    public UnitDamageInfo AddBossDamage(int addValue) => new UnitDamageInfo(_baseDamage, _baseBossDamage + addValue, _damageRate, _bossDamageRate);
    public UnitDamageInfo IncreaseDamageRate(float increaseValue) => new UnitDamageInfo(_baseDamage, _baseBossDamage, _damageRate + increaseValue, _bossDamageRate);
    public UnitDamageInfo IncreaseBossDamageRate(float increaseValue) => new UnitDamageInfo(_baseDamage, _baseBossDamage, _damageRate, _bossDamageRate + increaseValue);
}

public class UnitDamageInfoManager
{
    readonly Dictionary<UnitFlags, UnitDamageInfo> _damageInfoByFlag;
    readonly IReadOnlyDictionary<UnitFlags, UnitDamageInfo> OriginDamageInfoByFlag;

    public UnitDamageInfoManager(IReadOnlyDictionary<UnitFlags, UnitDamageInfo> originDamages)
    {
        int unitAllCount = Enum.GetValues(typeof(UnitColor)).Length * Enum.GetValues(typeof(UnitClass)).Length;
        Debug.Assert(originDamages.Count == unitAllCount, "유닛 스탯의 카운트가 올바르지 않음");
        OriginDamageInfoByFlag = originDamages;
        _damageInfoByFlag = new Dictionary<UnitFlags, UnitDamageInfo>(originDamages);
    }

    public UnitDamageInfo GetUpgradeInfo(UnitFlags flag)
    {
        var originInfo = OriginDamageInfoByFlag[flag];
        var currentInfo = _damageInfoByFlag[flag];

        return new UnitDamageInfo
            (currentInfo.BaseDamage - originInfo.BaseDamage,
            currentInfo.BaseBossDamage - originInfo.BaseBossDamage,
            currentInfo.DamageRate - originInfo.DamageRate,
            currentInfo.BossDamageRate - originInfo.BossDamageRate);
    }

    public UnitDamageInfo GetDamageInfo(UnitFlags flag) => _damageInfoByFlag[flag];
    public int GetUnitDamage(UnitFlags flag) => GetDamageInfo(flag).ApplyDamage;
    public int GetUnitBossDamage(UnitFlags flag) => GetDamageInfo(flag).ApplyBossDamage;
    public void AddDamage(UnitFlags flag, int addValue) => _damageInfoByFlag[flag] = _damageInfoByFlag[flag].AddDamage(addValue);
    public void AddBossDamage(UnitFlags flag, int addValue) => _damageInfoByFlag[flag] = _damageInfoByFlag[flag].AddBossDamage(addValue);
    public void IncreaseDamageRate(UnitFlags flag, float increaseValue) => _damageInfoByFlag[flag] = _damageInfoByFlag[flag].IncreaseDamageRate(increaseValue);
    public void IncreaseBossDamageRate(UnitFlags flag, float increaseValue) => _damageInfoByFlag[flag] = _damageInfoByFlag[flag].IncreaseBossDamageRate(increaseValue);

    public void AddDamage(UnitFlags flag, int value, UnitStatType changeStatType)
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

    public void ScaleDamage(UnitFlags flag, float value, UnitStatType changeStatType)
    {
        switch (changeStatType)
        {
            case UnitStatType.Damage: IncreaseDamageRate(flag, value); break;
            case UnitStatType.BossDamage: IncreaseBossDamageRate(flag, value); break;
            case UnitStatType.All:
                IncreaseDamageRate(flag, value);
                IncreaseBossDamageRate(flag, value);
                break;
        }
    }
}
