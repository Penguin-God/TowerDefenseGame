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
        public void 마나_충전량에_따라_가득_찼는지_알려줘야_함(int amount, bool expected)
        {
            var manaUseCase = CreateManaUseCase(100);
            AddMana(manaUseCase, amount);

            Assert.AreEqual(expected, manaUseCase.IsManaFull);
        }

        [Test]
        public void 마나를_더하면_현재_마나의_값을_반환해야_함()
        {
            var manaUseCase = CreateManaUseCase(100);
            Assert.AreEqual(30, AddMana(manaUseCase, 30));
            Assert.AreEqual(60, AddMana(manaUseCase, 30));
        }

        [Test]
        public void 마나를_초기화하면_현재_마나_값이_0이여야_함()
        {
            var manaUseCase = CreateManaUseCase(100);
            AddMana(manaUseCase, 50);
            manaUseCase.ClearMana();

            Assert.Zero(AddMana(manaUseCase, 0));
        }

        [Test]
        public void 마나가_잠겨_있으면_충전되면_안_됨()
        {
            var manaUseCase = CreateManaUseCase(100);
            manaUseCase.LockMana();
            Assert.Zero(AddMana(manaUseCase, 50));

            manaUseCase.ReleaseMana();
            Assert.AreEqual(50, AddMana(manaUseCase, 50));
        }
    }
}
