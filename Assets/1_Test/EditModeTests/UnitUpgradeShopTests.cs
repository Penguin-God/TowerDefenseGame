using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using System;

namespace Tests
{
    public class UnitUpgradeShopTests
    {
        [Test]
        public void 유닛_업그레이드_상점은_서로_다른_3개의_상품을_제공함()
        {
            var selector = new UnitUpgradeGoodsSelector();

            var result = selector.SelectGoodsSet();

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(3, result.Distinct().Count());
        }

        [Test]
        public void 상품이_랜덤하게_고루_뽑히는지_확인()
        {
            var selector = new UnitUpgradeGoodsSelector();
            var counter = new Dictionary<UnitUpgradeGoods, int>();
            int selectCount = 1000;

            for (int i = 0; i < selectCount; i++)
            {
                foreach (var goods in selector.SelectGoodsSet())
                {
                    if (counter.ContainsKey(goods))
                        counter[goods]++;
                    else
                        counter.Add(goods, 1);
                }
            }

            int allGoodsCount = Enum.GetValues(typeof(UnitUpgradeType)).Length *
                Enum.GetValues(typeof(UnitColor)).Cast<UnitColor>().Where(x => UnitFlags.SpecialColors.Contains(x) == false).Count();
            Assert.AreEqual(allGoodsCount, counter.Count);
            AssertRateDelta(counter, selectCount * 3);
        }

        [Test]
        public void 리롤_시_원래_있던_상품들은_제외해야_함()
        {
            var selector = new UnitUpgradeGoodsSelector();
            var goodsSet = CreateGoolsSet();
            int selectCount = 500;

            for (int i = 0; i < selectCount; i++)
            {
                var result = selector.SelectGoodsSetExcluding(goodsSet);
                Assert.IsEmpty(result.Intersect(goodsSet));
            }
        }

        [Test]
        public void 구매시_현재_상품들은_제외하고_뽑아야_함()
        {
            var goodsSet = CreateGoolsSet();
            int selectCount = 100;

            for (int i = 0; i < selectCount; i++)
                Assert.IsFalse(goodsSet.Contains(SelectGoodsExcluding(goodsSet)));
        }

        [Test]
        public void 구매한_상품을_제외해고_뽑은_경우_랜덤하게_뽑아야_함()
        {
            int selectCount = 1000;
            var counter = new Dictionary<UnitUpgradeGoods, int>();
            for (int i = 0; i < selectCount; i++)
            {
                var selectGoods = SelectGoodsExcluding(CreateGoolsSet());
                if (counter.ContainsKey(selectGoods))
                    counter[selectGoods]++;
                else
                    counter.Add(selectGoods, 1);
            }

            AssertRateDelta(counter, selectCount);
        }

        void AssertRateDelta(Dictionary<UnitUpgradeGoods, int> counter, int selectGoodsCount)
        {
            float expectRate = 1f / counter.Count;
            float delta = 0.1f;
            foreach (var item in counter.Values)
            {
                float selectRate = item / (float)selectGoodsCount;
                Assert.IsTrue(Mathf.Abs(selectRate - expectRate) < delta);
            }
        }

        IEnumerable<UnitUpgradeGoods> CreateGoolsSet() => new HashSet<UnitUpgradeGoods>()
        {
            new UnitUpgradeGoods(UnitUpgradeType.Value, UnitColor.Red),
            new UnitUpgradeGoods(UnitUpgradeType.Value, UnitColor.Blue),
            new UnitUpgradeGoods(UnitUpgradeType.Value, UnitColor.Yellow),
        };

        UnitUpgradeGoods SelectGoodsExcluding(IEnumerable<UnitUpgradeGoods> excludeGoodsSet)
            => new UnitUpgradeGoodsSelector().SelectGoodsExcluding(excludeGoodsSet);
    }
}
