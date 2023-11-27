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

    public UnitDamageInfo(int dam = 0, int bossDam = 0, float damRate = 0, float bossDamRate = 0)
    {
        _baseDamage = dam;
        _baseBossDamage = bossDam;
        _damageRate = damRate;
        _bossDamageRate = bossDamRate;
    }

    public static UnitDamageInfo CreateDamageInfo(int dam) => CreateDamageInfo(dam, dam);
    public static UnitDamageInfo CreateDamageInfo(int dam, int bossDam) => new UnitDamageInfo(dam, bossDam, 0, 0);
    public static UnitDamageInfo CreateRateInfo(float rate) => new UnitDamageInfo(0, 0, rate, rate);
    public static UnitDamageInfo CreateUpgradeInfo(int dam = 0, int bossDam = 0, float damRate = 0, float bossDamRate = 0) => new UnitDamageInfo(dam, bossDam, damRate, bossDamRate);

    public static UnitDamageInfo operator +(UnitDamageInfo a, UnitDamageInfo b)
    {
        a._baseDamage += b._baseDamage;
        a._baseBossDamage += b._baseBossDamage;
        a._damageRate += b._damageRate;
        a._bossDamageRate += b._bossDamageRate;
        return a;
    }
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
    public void AddDamage(UnitFlags flag, int addValue) => _damageInfoByFlag[flag] += new UnitDamageInfo(dam: addValue);
    public void AddBossDamage(UnitFlags flag, int addValue) => _damageInfoByFlag[flag] += new UnitDamageInfo(bossDam: addValue);
    public void IncreaseDamageRate(UnitFlags flag, float increaseValue) => _damageInfoByFlag[flag] += new UnitDamageInfo(damRate: increaseValue);
    public void IncreaseBossDamageRate(UnitFlags flag, float increaseValue) => _damageInfoByFlag[flag] += new UnitDamageInfo(bossDamRate: increaseValue);
    public void UpgradeDamageInfo(UnitFlags flag, UnitDamageInfo damageInfo) => _damageInfoByFlag[flag] += damageInfo;
}
