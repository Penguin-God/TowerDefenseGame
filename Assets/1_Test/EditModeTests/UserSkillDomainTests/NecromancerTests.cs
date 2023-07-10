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
        public void ��Ȱ_�õ�_������_ī��Ʈ��_�����ϰ�_����_����_������_��ȯ�ؾ�_��()
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
        public void ��Ȱ_����_��_����_��ȯ_��_ī��Ʈ_������_����_�ٽ�_������_��ȯ�ؾ�_��()
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
