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
        public void 얻은_경험치에_맞게_레벨이_올라가야_함(int expAmount, int expectedLV, int expectedExp)
        {
            var sut = CreateLevelSystem();

            sut.AddExperience(expAmount);

            Assert.AreEqual(expectedLV, sut.Level);
            Assert.AreEqual(expectedExp, sut.Experience);
        }

        [Test]
        public void 최대_레벨에_도달하면_경험치가_0으로_고정되야_함()
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
