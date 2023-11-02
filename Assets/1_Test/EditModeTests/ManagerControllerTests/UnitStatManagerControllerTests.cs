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
    WorldUnitDamageManager CreateWorldDamageManager() => new WorldUnitDamageManager(new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(DamageInfos)));
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

    WorldUnitDamageManager _worldUnitDamageManager;
    WorldUnitManager _worldUnitManager;

    [SetUp]
    public void Setup()
    {
        Debug.Log("@@");
        _worldUnitDamageManager = CreateWorldDamageManager();
        _worldUnitManager = CreateWorldUnitManager();
    }

    [Test]
    public void 대미지가_올라가면_같은ID_저장소의_데이터와_스폰된_유닛의_값이_같이_올라가야_함()
    {
        // Arrange
        var sut = new UnitStatController(_worldUnitDamageManager, _worldUnitManager);
        
        // Act
        sut.AddUnitDamage(RedSwordman, 300, UnitStatType.Damage, Id);

        // Assert
        AssertUnitDamage(400, RedSwordman);
        AssertUnitDamage(DefaultDamage, RedArcher);
    }

    [Test]
    public void 조건에_맞는_유닛_스탯만_강화되야_함()
    {
        // Arrange
        var sut = new UnitStatController(_worldUnitDamageManager, _worldUnitManager);
        
        // Act
        sut.AddUnitDamageWithColor(UnitColor.Red, 300, UnitStatType.Damage, Id);

        // Assert
        AssertUnitDamage(400, RedSwordman);
        AssertUnitDamage(400, RedArcher);
        AssertUnitDamage(100, BlueSwordman);

        // Act
        sut.ScaleUnitDamageWithColor(UnitColor.Red, 0.5f, UnitStatType.Damage, Id);

        // Assert
        AssertUnitDamage(600, RedSwordman);
        AssertUnitDamage(600, RedArcher);
        AssertUnitDamage(100, BlueSwordman);
    }

    void AssertUnitDamage(int upgradeDamage, UnitFlags flags)
    {
        Assert.AreEqual(upgradeDamage, _worldUnitDamageManager.GetUnitDamageInfo(flags, Id).ApplyDamage);
        Assert.AreEqual(upgradeDamage, _worldUnitManager.GetUnit(Id, flags).DamageInfo.ApplyDamage);
        Assert.AreEqual(upgradeDamage, _worldUnitManager.GetUnit(Id, flags).DamageInfo.ApplyDamage);

        Assert.AreEqual(DefaultDamage, _worldUnitDamageManager.GetUnitDamageInfo(flags, OtherId).ApplyDamage);
        Assert.AreEqual(DefaultDamage, _worldUnitManager.GetUnit(OtherId, flags).DamageInfo.ApplyDamage);
    }
}
