using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace WorldLogicTests 
{
    public class WorldUnitDamageManagerTests
    {
        const int DefaultDamage = 100;
        Dictionary<UnitFlags, UnitDamageInfo> CreateDamageInfos() => UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo(DefaultDamage, DefaultDamage));
        WorldUnitDamageManager CreateWorldDamageManager() => new WorldUnitDamageManager(new MultiData<UnitDamageInfoManager>(() => new UnitDamageInfoManager(CreateDamageInfos())));

        const byte id = 0;
        const byte otherId = 1;

        [Test]
        public void 조건에_맞는_유닛의_조건에_맞는_스탯만_강화되야_함()
        {
            var sut = CreateWorldDamageManager();

            sut.AddUnitDamageValue(x => x.UnitColor == UnitColor.Red, 100, UnitStatType.Damage, 0);

            AssertUnitDamage(sut, new UnitFlags(0, 0), 200);
            AssertUnitDamage(sut, new UnitFlags(0, 1), 200);
            AssertUnitDamage(sut, new UnitFlags(0, 2), 200);
            AssertUnitDamage(sut, new UnitFlags(0, 3), 200);
        }

        void AssertUnitDamage(WorldUnitDamageManager sut, UnitFlags unitFlags, int damage)
        {

            Assert.AreEqual(damage, sut.GetUnitDamageInfo(unitFlags, id).ApplyDamage);
            Assert.AreEqual(DefaultDamage, sut.GetUnitDamageInfo(unitFlags, id).ApplyBossDamage);
            Assert.AreEqual(DefaultDamage, sut.GetUnitDamageInfo(unitFlags, otherId).ApplyDamage);
        }
    }
}

