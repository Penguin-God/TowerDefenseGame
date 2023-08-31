using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ShopLogicTests
{
    public class GoodsManagerTests
    {
        readonly IReadOnlyList<string> _initialGoods = new List<string>
        {
            "Apple",
            "Banana",
            "Cherry",
            "Orange",
            "Pizaa",
        };

        public GoodsManager<string> CreateGoodsManager() => new GoodsManager<string>(new HashSet<string>(_initialGoods));

        [Test]
        public void Ȯ��_����()
        {
            for (int i = 0; i < 1000; i++)
            {
                ������_��ǰ��_�̾ƾ�_�ϸ�_�Ȱ���_��ǰ��_�ٽ�_������_��_��();
                ��ǰ_��ȯ_��_�ٸ�_����_���_��();
            }
        }

        [Test]
        public void ������_��ǰ��_�̾ƾ�_�ϸ�_�Ȱ���_��ǰ��_�ٽ�_������_��_��()
        {
            var sut = CreateGoodsManager();

            var result = sut.GetRandomGoods();

            CollectionAssert.Contains(_initialGoods, result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
        }

        [Test]
        public void ��ǰ_��ȯ_��_�ٸ�_����_���_��()
        {
            var sut = CreateGoodsManager();
            var firstGood = sut.GetRandomGoods();
            var changedGood = sut.ChangeGoods(firstGood);

            Assert.AreNotEqual(firstGood, changedGood);
        }

        [Test]
        public void ���_��ǰ_��ȯ��_����_�ٸ�_��ǰ��_�÷�����_���_��()
        {
            var sut = CreateGoodsManager();
            IEnumerable<string> GoodsList = new List<string>
            {
                sut.GetRandomGoods(),
                sut.GetRandomGoods()
            };

            var newGoodsList = sut.ChangeAllGoods();

            CollectionAssert.AreNotEquivalent(GoodsList, newGoodsList);
            Assert.AreEqual(GoodsList.Count(), newGoodsList.Count());
            Assert.IsFalse(GoodsList.Any(goods => newGoodsList.Contains(goods)));
        }
    }
}
