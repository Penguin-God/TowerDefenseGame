using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitDomainTests
{
    public class ManaTests
    {
        ManaUseCase CreateManaUseCase(int maxMana) => new ManaUseCase(maxMana);
        int AddMana(ManaUseCase mana, int amount) => mana.AddMana(amount);
        
        [Test]
        [TestCase(100, true)]
        [TestCase(80, false)]
        public void ����_��������_����_����_á����_�˷����_��(int amount, bool expected)
        {
            var manaUseCase = CreateManaUseCase(100);
            AddMana(manaUseCase, amount);

            Assert.AreEqual(expected, manaUseCase.IsManaFull);
        }

        [Test]
        public void ������_���ϸ�_����_������_����_��ȯ�ؾ�_��()
        {
            var manaUseCase = CreateManaUseCase(100);
            Assert.AreEqual(30, AddMana(manaUseCase, 30));
            Assert.AreEqual(60, AddMana(manaUseCase, 30));
        }

        [Test]
        public void ������_�ʱ�ȭ�ϸ�_����_����_����_0�̿���_��()
        {
            var manaUseCase = CreateManaUseCase(100);
            AddMana(manaUseCase, 50);
            manaUseCase.ClearMana();

            Assert.Zero(AddMana(manaUseCase, 0));
        }

        [Test]
        public void ������_���_������_�����Ǹ�_��_��()
        {
            var manaUseCase = CreateManaUseCase(100);
            manaUseCase.LockMana();
            Assert.Zero(AddMana(manaUseCase, 50));

            manaUseCase.ReleaseMana();
            Assert.AreEqual(50, AddMana(manaUseCase, 50));
        }
    }
}
