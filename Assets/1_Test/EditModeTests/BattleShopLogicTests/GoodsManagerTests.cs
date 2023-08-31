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
        public void 확률_검증()
        {
            for (int i = 0; i < 1000; i++)
            {
                랜덤한_상품을_뽑아야_하며_똑같은_상품을_다시_뽑으면_안_됨();
                상품_교환_시_다른_것을_줘야_함();
            }
        }

        [Test]
        public void 랜덤한_상품을_뽑아야_하며_똑같은_상품을_다시_뽑으면_안_됨()
        {
            var sut = CreateGoodsManager();

            var result = sut.GetRandomGoods();

            CollectionAssert.Contains(_initialGoods, result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
        }

        [Test]
        public void 상품_교환_시_다른_것을_줘야_함()
        {
            var sut = CreateGoodsManager();
            var firstGood = sut.GetRandomGoods();
            var changedGood = sut.ChangeGoods(firstGood);

            Assert.AreNotEqual(firstGood, changedGood);
        }

        [Test]
        public void 모든_상품_교환은_전부_다른_상품의_컬랙션을_줘야_함()
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
