using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MultiServiceTests
    {
        [Test]
        public void 서비스는_서로_다른_객체를_생성해야_함()
        {
            var sut = new MultiService<UnitManager>(() => new UnitManager());

            Assert.AreNotSame(sut.GetServiece(0), sut.GetServiece(1));
        }

        [Test]
        public void ID에_알맞은_서비스를_제공해야_함()
        {
            var sut = new MultiService<UnitManager>(() => new UnitManager());

            sut.GetServiece(0).RegisterUnit(new Unit(UnitFlags.RedSowrdman));

            Assert.AreEqual(sut.GetServiece(0).UnitCount, 1);
            Assert.AreEqual(sut.GetServiece(1).UnitCount, 0);
        }
    }
}
