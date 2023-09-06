using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace UserSkillDomainTests
{
    public class GamblerLevelSystemTests
    {
        GamblerLevelSystem CreateSut()
        {
            int[] experienceToNextLevel = { 100, 200, 300 }; // ���� ���Դϴ�.
            var levelSystem = new LevelSystem(experienceToNextLevel);
            return new GamblerLevelSystem(levelSystem);
        }

        void LevelUp(GamblerLevelSystem levelSystem) => levelSystem.LevelUp();

        [Test]
        public void ������_����_������_�����Ǹ鼭_���_����_�õ���_����ġ_�ʱ�ȭ��_����Ǿ�_��()
        {
            var sut = CreateSut();

            sut.AddExperience(600);
            LevelUp(sut);
            LevelUp(sut);
            LevelUp(sut);
            Assert.AreEqual(3, sut.Level);

            sut.AddExperience(350);
            Assert.AreEqual(350, sut.Experience);
            LevelUp(sut);
            Assert.AreEqual(50, sut.Experience);
            Assert.AreEqual(3, sut.Level);
        }
    }
}
