using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UserSkillDomainTests
{
    public class LevelUpTests
    {
        LevelSystem CreateLevelSystem() => new LevelSystem(new int[] { 100, 200, 300 });

        [Test]
        [TestCase(20, 1, 20)]
        [TestCase(150, 2, 50)]
        [TestCase(340, 3, 40)]
        public void ����_����ġ��_�°�_������_�ö󰡾�_��(int expAmount, int expectedLV, int expectedExp)
        {
            var sut = CreateLevelSystem();

            sut.AddExperience(expAmount);

            Assert.AreEqual(expectedLV, sut.Level);
            Assert.AreEqual(expectedExp, sut.Experience);
        }

        [Test]
        public void �ִ�_������_�����ϸ�_����ġ��_0����_�����Ǿ�_��()
        {
            var sut = CreateLevelSystem();

            sut.AddExperience(1000);
            Assert.AreEqual(4, sut.Level);
            Assert.AreEqual(0, sut.Experience);

            sut.AddExperience(1000);
            Assert.AreEqual(4, sut.Level);
            Assert.AreEqual(0, sut.Experience);
        }
    }
}
