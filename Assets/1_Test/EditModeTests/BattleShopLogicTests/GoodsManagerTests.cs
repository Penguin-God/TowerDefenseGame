using System.Collections;
using System.Collections.Generic;
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
            "Cherry"
        };

        public GoodsManager<string> CreateGoodsManager() => new GoodsManager<string>(new HashSet<string>(_initialGoods));

        [Test]
        public void 랜덤한_상품을_뽑아야_하며_똑같은_상품을_다시_뽑으면_안_됨()
        {
            var sut = CreateGoodsManager();

            var result = sut.GetRandomGoods();

            CollectionAssert.Contains(_initialGoods, result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
            CollectionAssert.DoesNotContain(sut.GetRandomGoods(), result);
        }
    }
}
