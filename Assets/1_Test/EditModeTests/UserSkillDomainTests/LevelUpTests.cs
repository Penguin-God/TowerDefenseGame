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
        bool LevelUp(LevelSystem levelSystem) => levelSystem.LevelUp();
        void AddExp(LevelSystem levelSystem, int amount) => levelSystem.AddExperience(amount);

        [Test]
        [TestCase(20, 1, 20, false)]
        [TestCase(150, 2, 50, true)]
        [TestCase(340, 2, 240, true)]
        public void ����_����ġ��ŭ_��������_�����ؾ�_��(int expAmount, int expectedLV, int expectedExp, bool isLevelUp)
        {
            var sut = CreateLevelSystem();

            AddExp(sut, expAmount);
            bool result = LevelUp(sut);

            Assert.AreEqual(isLevelUp, result);
            Assert.AreEqual(expectedLV, sut.Level);
            Assert.AreEqual(expectedExp, sut.Experience);
        }

        [Test]
        public void �ִ�_������_�����ϸ�_����ġ��_0����_�����Ǿ�_��()
        {
            var sut = CreateLevelSystem();

            AddExp(sut, 1000);
            LevelUp(sut);
            LevelUp(sut);
            Assert.AreEqual(3, sut.Level);
            Assert.AreEqual(700, sut.Experience);

            LevelUp(sut);
            Assert.AreEqual(4, sut.Level);
            Assert.AreEqual(0, sut.Experience);
        }

        [Test]
        public void ��Ȳ��_�´�_�̹�Ʈ��_����_��()
        {
            var sut = CreateLevelSystem();
            
            int experience = 0;
            sut.OnChangeExp += (exp) => experience = exp;
            AddExp(sut, 150);
            Assert.AreEqual(150, experience);

            int level = 0;
            sut.OnLevelUp += (lv) => level = lv;
            LevelUp(sut);
            Assert.AreEqual(2, level);
        }
    }
}
