using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    public class TestUnitStat
    {
        Dictionary<UnitFlags, UnitDamageInfo> CreateDamageInfos()
        {
            var damageInfoByFlag = new Dictionary<UnitFlags, UnitDamageInfo>();
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                    damageInfoByFlag.Add(new UnitFlags(color, unitClass), new UnitDamageInfo(0, 0));
            }
            return damageInfoByFlag;
        }
        UnitFlags RedSwordman => new UnitFlags(0, 0);

        [Test]
        public void CreateStatManager()
        {
            var manager = new UnitStatManager(CreateDamageInfos());

            // 모든 유닛 정보가 잘 가져와지는지 확인
            foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
            {
                foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                {
                    Assert.AreEqual(0, manager.GetUnitDamage(new UnitFlags(color, unitClass)));
                    Assert.AreEqual(0, manager.GetUnitBossDamage(new UnitFlags(color, unitClass)));
                }
            }
        }

        [Test]
        public void AddBaseDamage()
        {
            var manager = new UnitStatManager(CreateDamageInfos());

            manager.AddDamage(RedSwordman, 300);

            Assert.AreEqual(300, manager.GetUnitDamage(RedSwordman));
        }

        [Test]
        public void AddBaseBossDamage()
        {
            var manager = new UnitStatManager(CreateDamageInfos());

            manager.AddBossDamage(RedSwordman, 300);

            Assert.AreEqual(300, manager.GetUnitBossDamage(RedSwordman));
        }

        [Test]
        public void AddDamageRate()
        {
            var infos = CreateDamageInfos();
            infos[RedSwordman] = new UnitDamageInfo(100, 0);
            var manager = new UnitStatManager(infos);

            manager.IncreaseDamageRate(RedSwordman, 0.5f);

            Assert.AreEqual(150, manager.GetUnitDamage(RedSwordman));
        }

        [Test]
        public void AddBossDamageRate()
        {
            var infos = CreateDamageInfos();
            infos[RedSwordman] = new UnitDamageInfo(0, 100);
            var manager = new UnitStatManager(infos);

            manager.IncreaseBossDamageRate(RedSwordman, 0.5f);

            Assert.AreEqual(150, manager.GetUnitBossDamage(RedSwordman));
        }
    }
}
