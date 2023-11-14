using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MonsterDomainTests
{
    public class MonsterHpByteConvertorTests
    {
        readonly MonsterHpByteConvertor Convertor = new();

        [Test]
        [TestCase(30, 510, 15)]
        [TestCase(100, 200, 127)]
        [TestCase(400, 400, 255)]
        [TestCase(0, 510, 0)]
        public void 현제_체력을_255분율로_계산해야_함(int hp, int maxHp, byte expected)
        {
            // Act
            byte result = Convertor.CalculateHealthByte(hp, maxHp);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
