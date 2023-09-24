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
        public void ����_����_��_���̵�_�ݴ�������_�ٲ���_��(byte startId, byte expected)
        {
            var spot = new ObjectSpot(startId, true);

            Assert.AreEqual(expected, spot.ChangeWorldId().WorldId);
        }
    }
}
