using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Linq;

namespace Tests
{
    public class DamageInfoManagerTests
    {
        const int DefaultDamage = 100;
        Dictionary<UnitFlags, UnitDamageInfo> CreateDamageInfos() => UnitFlags.AllFlags.ToDictionary(x => x, x => new UnitDamageInfo(DefaultDamage, DefaultDamage));
        UnitDamageInfoManager CreateInfoManager() => new UnitDamageInfoManager(CreateDamageInfos());
        int GetRedSwordmanDamage(UnitDamageInfoManager manager) => manager.GetUnitDamage(RedSwordman);
        int GetRedSwordmanBossDamage(UnitDamageInfoManager manager) => manager.GetUnitBossDamage(RedSwordman);
        UnitFlags RedSwordman => new UnitFlags(0, 0);

        [Test]
        public void CreateStatManager()
        {
            var manager = CreateInfoManager();

            // 모든 유닛 정보가 잘 가져와지는지 확인
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                {
                    Assert.AreEqual(DefaultDamage, manager.GetUnitDamage(new UnitFlags(color, unitClass)));
                    Assert.AreEqual(DefaultDamage, manager.GetUnitBossDamage(new UnitFlags(color, unitClass)));
                }
            }
        }

        [Test]
        [TestCase(50, 0, 150, 100)]
        [TestCase(0, 50, 100, 150)]
        [TestCase(50, 50, 150, 150)]
        public void 합_증가는_원본_대미지에_더하기로_적용됨(int damValue, int bossDamValue, int dam, int bossDam)
        {
            var manager = CreateInfoManager();

            manager.AddDamage(RedSwordman, damValue);
            manager.AddBossDamage(RedSwordman, bossDamValue);

            Assert.AreEqual(dam, GetRedSwordmanDamage(manager));
            Assert.AreEqual(bossDam, GetRedSwordmanBossDamage(manager));
            Assert.AreEqual(damValue, manager.GetUpgradeInfo(RedSwordman).BaseDamage);
            Assert.AreEqual(bossDamValue, manager.GetUpgradeInfo(RedSwordman).BaseBossDamage);
        }

        [Test]
        [TestCase(0.5f, 0, 150, 100)]
        [TestCase(0f, 0.5f, 100, 150)]
        [TestCase(0.5f, 0.5f, 150, 150)]
        public void 퍼센트_증가는_원본_대미지에_배율로_적용됨(float damRate, float bossDamRate, int dam, int bossDam)
        {
            var manager = CreateInfoManager();

            manager.IncreaseDamageRate(RedSwordman, damRate);
            manager.IncreaseBossDamageRate(RedSwordman, bossDamRate);

            Assert.AreEqual(dam, GetRedSwordmanDamage(manager));
            Assert.AreEqual(bossDam, GetRedSwordmanBossDamage(manager));
            Assert.AreEqual(damRate, manager.GetUpgradeInfo(RedSwordman).DamageRate);
            Assert.AreEqual(bossDamRate, manager.GetUpgradeInfo(RedSwordman).BossDamageRate);
        }

        [Test]
        public void 퍼센트_증가는_합적용임()
        {
            var manager = CreateInfoManager();

            manager.IncreaseBossDamageRate(RedSwordman, 0.5f);
            manager.IncreaseBossDamageRate(RedSwordman, 0.5f);

            Assert.AreEqual(200, GetRedSwordmanBossDamage(manager));
            Assert.AreEqual(1, manager.GetUpgradeInfo(RedSwordman).BossDamageRate);
        }

        [Test]
        public void 퍼센트랑_깡공_둘_다_증가_시_추가된_기본_대미지도_배율이_적용되야_함()
        {
            var manager = CreateInfoManager();

            manager.AddDamage(RedSwordman, 100);
            manager.IncreaseDamageRate(RedSwordman, 0.5f);

            Assert.AreEqual(300, GetRedSwordmanDamage(manager));
        }
    }
}
