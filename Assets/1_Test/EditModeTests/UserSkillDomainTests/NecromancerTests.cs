using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UserSkillDomainTests
{
    public class NecromancerTests
    {
        const int NeedKillCountForSummon = 20;
        Necromencer CreateNecromancer() => new Necromencer(NeedKillCountForSummon);

        [Test]
        public void 부활_시도_떄마다_카운트가_증가하고_못할_때는_거짓을_반환해야_함()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
            {
                Assert.AreEqual(i, sut.CurrentKillCount);
                bool result = sut.TryResurrect();
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void 부활_가능_시_참을_반환_후_카운트_리셋한_다음_다시_거짓을_반환해야_함()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
                sut.TryResurrect();

            Assert.IsTrue(sut.TryResurrect());
            Assert.Zero(sut.CurrentKillCount);
            Assert.IsFalse(sut.TryResurrect());
        }
    }
}
