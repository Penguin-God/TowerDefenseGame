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
            int[] experienceToNextLevel = { 100, 200, 300 }; // 예시 값입니다.
            var levelSystem = new LevelSystem(experienceToNextLevel);
            return new GamblerLevelSystem(levelSystem);
        }

        void LevelUp(GamblerLevelSystem levelSystem) => levelSystem.LevelUp();

        [Test]
        public void 만렙일_때도_레벨을_유지되면서_계속_렙업_시도와_경험치_초기화가_진행되야_함()
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
