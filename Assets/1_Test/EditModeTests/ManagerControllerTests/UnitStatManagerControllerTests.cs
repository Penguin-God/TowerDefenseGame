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
    Dictionary<UnitFlags, UnitDamageInfo> DamageInfos = UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo(DefaultDamage, DefaultDamage));
    WorldUnitDamageManager CreateWorldDamageManager() => new WorldUnitDamageManager(new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(DamageInfos)));
    readonly UnitFlags RedSwordman = new UnitFlags(0, 0);

    WorldUnitManager CreateWorldUnitManager()
    {
        var result = new WorldUnitManager();
        result.AddUnit(CreateUnit(0, 0), 0);
        result.AddUnit(CreateUnit(0, 1), 0);

        result.AddUnit(CreateUnit(0, 0), 1);
        result.AddUnit(CreateUnit(0, 1), 1);
        return result;
    }

    [Test]
    public void 대미지가_올라가면_저장소의_데이터와_현재_유닛의_값이_같이_올라가야_함()
    {
        // Arrange
        var worldUnitDamageManager = CreateWorldDamageManager();
        var worldUnitManager = CreateWorldUnitManager();
        var sut = new UnitStatController(worldUnitDamageManager, worldUnitManager);
        int value = 300;

        // Act
        sut.AddUnitDamageWithFlag(RedSwordman, value, UnitStatType.Damage, Id);

        // Assert
        Assert.AreEqual(400, worldUnitManager.GetUnit(Id, RedSwordman).DamageInfo.ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitManager.GetUnit(Id, new UnitFlags(0, 1)).DamageInfo.ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitManager.GetUnit(OtherId, RedSwordman).DamageInfo.ApplyDamage);

        Assert.AreEqual(400, worldUnitDamageManager.GetUnitDamageInfo(RedSwordman, Id).ApplyDamage);
        Assert.AreEqual(DefaultDamage, worldUnitDamageManager.GetUnitDamageInfo(RedSwordman, OtherId).ApplyDamage);
    }
}
