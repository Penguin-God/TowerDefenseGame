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
        public void ��Ȱ_����_����_������_��ȯ�ؾ�_��()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
            {
                bool result = sut.TryResurrect();
                Assert.IsFalse(result);
            }
        }

        [Test]
        public void ��Ȱ_����_��_����_��ȯ��_�ٽ�_������_��ȯ�ؾ�_��()
        {
            var sut = CreateNecromancer();

            for (int i = 0; i < NeedKillCountForSummon - 1; i++)
                sut.TryResurrect();

            Assert.IsTrue(sut.TryResurrect());
            Assert.IsFalse(sut.TryResurrect());
        }
    }
}
