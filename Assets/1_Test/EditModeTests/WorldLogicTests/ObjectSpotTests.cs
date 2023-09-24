using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace WorldLogicTests
{
    public class ObjectSpotTests
    {
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public void 월드_변경_시_아이디가_반대쪽으로_바뀌어야_함(byte startId, byte expected)
        {
            var spot = new ObjectSpot(startId, true);

            Assert.AreEqual(expected, spot.ChangeWorldId().WorldId);
        }
    }
}
