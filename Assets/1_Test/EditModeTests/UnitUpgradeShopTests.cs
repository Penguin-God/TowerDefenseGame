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
            var counter = new Dictionary<UnitUpgradeGoodsData, int>();
            int selectCount = 1000;

            for (int i = 0; i < selectCount; i++)
            {
                var goods = selector.SelectGoodsSet().First();
                if (counter.ContainsKey(goods))
                    counter[goods]++;
                else
                    counter.Add(goods, 1);
            }

            // 모든 상품을 포함하는지 확인
            int allGoodsCount = Enum.GetValues(typeof(UnitUpgradeType)).Length *
                Enum.GetValues(typeof(UnitColor)).Cast<UnitColor>().Where(x => UnitFlags.SpecialColors.Contains(x) == false).Count();
            Assert.AreEqual(allGoodsCount, counter.Count);
            // 확률 분포 확인
            float expectRate = 1f / counter.Count;
            float delta = 0.1f;
            foreach (var item in counter.Values)
            {
                float selectRate = item / (float)selectCount;
                Assert.IsTrue(Mathf.Abs(selectRate - expectRate) < delta);
            }
        }

        [Test]
        public void 제외하고_싶은_상품들은_빼고_선택해야_함()
        {
            var selector = new UnitUpgradeGoodsSelector();
            var goodsSet = CreateRedBlueYellowValueGoolsSet();
            int selectCount = 500;
            
            for (int i = 0; i < selectCount; i++)
                CollectionAssert.DoesNotContain(goodsSet, selector.SelectGoodsExcluding(goodsSet));
        }

        HashSet<UnitUpgradeGoodsData> CreateRedBlueYellowValueGoolsSet() => new HashSet<UnitUpgradeGoodsData>()
        {
            new UnitUpgradeGoodsData(UnitUpgradeType.Value, UnitColor.Red),
            new UnitUpgradeGoodsData(UnitUpgradeType.Value, UnitColor.Blue),
            new UnitUpgradeGoodsData(UnitUpgradeType.Value, UnitColor.Yellow),
        };
    }
}
