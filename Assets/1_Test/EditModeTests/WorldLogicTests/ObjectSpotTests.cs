using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace WorldLogicTests
{
    public class ObjectSpotTests
    {
        ObjectSpot CreateSpot(byte id, bool isInDefenseWorld) => new ObjectSpot(id, isInDefenseWorld);

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void 월드_변경_시_아이디가_반대쪽으로_바뀌어야_함(byte startId, byte expected)
        {
            var spot = CreateSpot(startId, true);

            Assert.AreEqual(expected, spot.ChangeWorldId().WorldId);
        }

        [Test]
        [TestCase(0, true, 0, true, true)]
        [TestCase(1, false, 1, false, true)]
        [TestCase(0, true, 1, true, false)]
        [TestCase(1, false, 1, true, false)]
        [TestCase(0, false, 1, true, false)]
        public void 같은_값을_들고_있으면_동일한_것으로_판단해야_함(byte startId1, bool startWorld1, byte startId2, bool startWorld2, bool expected)
        {
            bool result = CreateSpot(startId1, startWorld1) == CreateSpot(startId2, startWorld2);
            Assert.AreEqual(expected, result);
        }
    }
}
