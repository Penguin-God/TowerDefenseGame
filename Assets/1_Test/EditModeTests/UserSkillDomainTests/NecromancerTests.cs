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
        public void 부활_못할_때는_거짓을_반환해야_함()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
            {
                bool result = sut.TryResurrect();
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void 부활_가능_시_참을_반환후_다시_거짓을_반환해야_함()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
                sut.TryResurrect();

            Assert.IsTrue(sut.TryResurrect());
            Assert.IsFalse(sut.TryResurrect());
        }
    }
}
