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
    Unit CreateUnit(int color, int unitClass) => new Unit(new UnitFlags(color, unitClass), new UnitStats(new UnitDamageInfo(DefaultDamage, DefaultDamage), 0, 0, 0, 0));
    UnitManager CreateUnitManager()
    {
        var result = new UnitManager();
        result.AddObject(CreateUnit(0, 0));
        result.AddObject(CreateUnit(0, 1));
        return result;
    }
    Dictionary<UnitFlags, UnitDamageInfo> DamageInfos = UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo(DefaultDamage, DefaultDamage));
    WorldUnitDamageManager CreateWorldDamageManager() => new WorldUnitDamageManager(new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(DamageInfos)));
    readonly UnitFlags RedSwordman = new UnitFlags(0, 0);

    [Test]
    public void 대미지가_올라가면_저장소의_데이터와_현재_유닛의_값이_같이_올라가야_함()
    {
        // Arrange
        var worldUnitManager = new MultiData<UnitManager>(CreateUnitManager);
        var worldUnitDamageManager = CreateWorldDamageManager();
        var sut = new UnitStatController(worldUnitDamageManager, worldUnitManager);
        int value = 300;

        // Act
        sut.AddUnitDamageWithFlag(RedSwordman, value, UnitStatType.Damage, Id);

        // Assert
        Assert.AreEqual(400, worldUnitManager.GetData(Id).FindUnit(RedSwordman).DamageInfo.ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitManager.GetData(Id).FindUnit(new UnitFlags(0, 1)).DamageInfo.ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitManager.GetData(OtherId).FindUnit(RedSwordman).DamageInfo.ApplyDamage);

        Assert.AreEqual(400, worldUnitDamageManager.GetUnitDamageInfo(RedSwordman, Id).ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitDamageManager.GetUnitDamageInfo(RedSwordman, OtherId).ApplyDamage);
    }
}
