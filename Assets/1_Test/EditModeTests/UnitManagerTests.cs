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
        public void 유닛이_매니저에_추가되면_찾을_수_있어야함()
        {
            var sut = new UnitManager();

            sut.RegisterUnit(new Unit(UnitFlags.RedSowrdman));

            Assert.AreEqual(1, sut.UnitCount);
            Assert.NotNull(sut.GetUnit(UnitFlags.RedSowrdman));
            Assert.IsNull(sut.GetUnit(UnitFlags.BlueSowrdman));
        }

        [Test]
        public void 유닛이_죽으면_매니저에서_삭제되어야함()
        {
            var sut = new UnitManager();
            var unit = new Unit(UnitFlags.RedSowrdman);
            sut.RegisterUnit(unit);

            unit.Dead();

            Assert.AreEqual(0, sut.UnitCount);
            Assert.IsNull(sut.GetUnit(UnitFlags.RedSowrdman));
        }
    }
}
