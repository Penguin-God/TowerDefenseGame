using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TextValueSystemTests
    {
        UnitFlags RedArcher = new UnitFlags(0, 1);
        UnitFlags VioaltSpearman = new UnitFlags(5, 2);

        [Test]
        public void ����_�÷��׿�_����_������_Ű��_�����ؾ�_��()
        {
            var sut = new UnitKeyBuilder();

            Assert.AreEqual("{%At01}", sut.BuildAttackKey(RedArcher));
            Assert.AreEqual("{%BAt01}", sut.BuildBossAttackKey(RedArcher));
        }

        [Test]
        public void �нú��_������ŭ��_Ű��_�����ؾ�_��()
        {
            var sut = new UnitKeyBuilder();
            const int PassiveCount = 4;

            var result = sut.BuildPassiveKeys(VioaltSpearman, PassiveCount).ToList();

            Assert.AreEqual(result.Count, PassiveCount);
            Assert.AreEqual("{%Pa520}", result[0]);
            Assert.AreEqual("{%Pa521}", result[1]);
            Assert.AreEqual("{%Pa522}", result[2]);
            Assert.AreEqual("{%Pa523}", result[3]);
        }

        [Test]
        public void ���_Ű��_�����ؾ�_��()
        {
            var sut = new UnitKeyBuilder();
            const int PassiveCount = 4;

            var result = sut.BuildAllKeys(VioaltSpearman, PassiveCount);

            Assert.AreEqual(result.Count(), 6);

            CollectionAssert.Contains(result, "{%At52}");
            CollectionAssert.Contains(result, "{%BAt52}");
            CollectionAssert.Contains(result, "{%Pa520}");
            CollectionAssert.Contains(result, "{%Pa521}");
            CollectionAssert.Contains(result, "{%Pa522}");
            CollectionAssert.Contains(result, "{%Pa523}");
        }
    }
}
