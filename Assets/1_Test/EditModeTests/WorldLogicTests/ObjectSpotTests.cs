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
        public void ����_����_��_���̵�_�ݴ�������_�ٲ���_��(byte startId, byte expected)
        {
            var spot = CreateSpot(startId, true);

            Assert.AreEqual(expected, spot.ChangeWorldId().WorldId);
        }

        [Test]
        [TestCase(0, true)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(1, false)]
        public void ����_����_���_������_������_������_�Ǵ��ؾ�_��(byte startId, bool startWorld)
        {
            Assert.AreEqual(CreateSpot(startId, startWorld), CreateSpot(startId, startWorld));
        }
    }
}
