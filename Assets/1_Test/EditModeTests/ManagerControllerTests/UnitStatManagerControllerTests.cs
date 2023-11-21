using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitStatManagerControllerTests
{
    const int DefaultDamage = 100;
    const byte Id = 0;
    const byte OtherId = 1;
    Unit CreateUnit(UnitFlags flag) => new Unit(flag, new UnitStats(new UnitDamageInfo(DefaultDamage, DefaultDamage), 0, 0, 0, 0));
    Dictionary<UnitFlags, UnitDamageInfo> DamageInfos = UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo(DefaultDamage, DefaultDamage));
    MultiData<UnitDamageInfoManager> CreateInfoManagers() => new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(DamageInfos));

    readonly UnitFlags RedSwordman = new UnitFlags(0, 0);
    readonly UnitFlags RedArcher = new UnitFlags(0, 1);
    readonly UnitFlags BlueSwordman = new UnitFlags(1, 0);
    WorldUnitManager CreateWorldUnitManager()
    {
        var result = new WorldUnitManager();
        result.AddUnit(CreateUnit(RedSwordman), Id);
        result.AddUnit(CreateUnit(RedArcher), Id);
        result.AddUnit(CreateUnit(BlueSwordman), Id);

        result.AddUnit(CreateUnit(RedSwordman), OtherId);
        result.AddUnit(CreateUnit(RedArcher), OtherId);
        result.AddUnit(CreateUnit(BlueSwordman), OtherId);
        return result;
    }

    WorldUnitManager _worldUnitManager;
    UnitStatController sut;
    [SetUp]
    public void Setup()
    {
        _worldUnitManager = CreateWorldUnitManager();
        sut = new UnitStatController(CreateInfoManagers(), _worldUnitManager);
    }
    bool SameRedSwordman(UnitFlags flag) => flag == RedSwordman;
    void UpgradeRedSwordman(UnitDamageInfo info) => sut.UpgradeUnitDamage(SameRedSwordman, info, Id);

    [Test]
    public void 조건에_맞는_유닛만_강화되야_함()
    {
        // Act
        UpgradeRedSwordman(new UnitDamageInfo(dam: 300));

        // Assert
        AssertUnitDamage(400, RedSwordman);
        AssertUnitDamage(100, RedArcher);
        AssertUnitDamage(100, BlueSwordman);

        // Act
        UpgradeRedSwordman(new UnitDamageInfo(damRate: 0.5f));

        // Assert
        AssertUnitDamage(600, RedSwordman);
        AssertUnitDamage(100, RedArcher);
        AssertUnitDamage(100, BlueSwordman);
    }

    void AssertUnitDamage(int upgradeDamage, UnitFlags flag)
    {
        Assert.AreEqual(upgradeDamage, sut.GetDamageInfo(flag, Id).ApplyDamage);
        Assert.AreEqual(upgradeDamage, _worldUnitManager.GetUnit(Id, flag).DamageInfo.ApplyDamage);
        Assert.AreEqual(upgradeDamage, _worldUnitManager.GetUnit(Id, flag).DamageInfo.ApplyDamage);

        Assert.AreEqual(DefaultDamage, sut.GetDamageInfo(flag, OtherId).ApplyDamage);
        Assert.AreEqual(DefaultDamage, _worldUnitManager.GetUnit(OtherId, flag).DamageInfo.ApplyDamage);
    }

    [Test]
    [TestCase(100, 100, 0, 0, 200, 200)]
    [TestCase(100, 100, 0.5f, 0.5f, 300, 300)]
    [TestCase(100, 0, 0.5f, 0, 300, 100)]
    [TestCase(0, 0, 0, 0.5f, 100, 150)]
    public void 조건에_맞는_스탯만_강화되야_함(int dam, int bossDam, float damRate, float bossDamRate, int expectDam, int expectBossDam)
    {
        var sut = new UnitDamageInfoManager(DamageInfos);

        sut.UpgradeDamageInfo(RedSwordman, new UnitDamageInfo(dam, bossDam, damRate, bossDamRate));

        var result = sut.GetDamageInfo(RedSwordman);
        Assert.AreEqual(expectDam, result.ApplyDamage);
        Assert.AreEqual(expectBossDam, result.ApplyBossDamage);
    }

    [Test]
    public void UnitDamageInfo_구조체끼리는_더할_수_있음()
    {
        var a = new UnitDamageInfo(2, 2, 1, 1);
        var b = new UnitDamageInfo(0, 3, 2, 0.1f);
        var result = a + b;
        Assert.AreEqual(new UnitDamageInfo(2, 5, 3, 1.1f), result);
    }
}
