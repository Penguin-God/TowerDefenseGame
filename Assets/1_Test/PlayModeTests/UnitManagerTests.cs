using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Spawners;

namespace Tests
{
    public class UnitManagerTests
    {
        [Test]
        public void 유닛이_스폰되면_매니저에_추가되어야함()
        {
            var spawner = new UnitSpanwer(Managers.Resources);
            var sut = new UnitManager(spawner);

            spawner.Spawn(UnitFlags.RedSowrdman);

            Assert.AreEqual(1, sut.UnitCount);
            Assert.NotNull(sut.GetUnit(UnitFlags.RedSowrdman));
            Assert.IsNull(sut.GetUnit(UnitFlags.BlueSowrdman));
        }

        [Test]
        public void 유닛이_죽으면_매니저에서_삭제되어야함()
        {
            var spawner = new UnitSpanwer(Managers.Resources);
            var sut = new UnitManager(spawner);
            var unit = spawner.Spawn(UnitFlags.RedSowrdman);
            
            unit.OnDead?.Invoke(unit);

            Assert.AreEqual(0, sut.UnitCount);
        }
    }
}
