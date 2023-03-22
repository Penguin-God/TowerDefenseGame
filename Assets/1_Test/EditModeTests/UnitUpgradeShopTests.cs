using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.Data.Common;

namespace Tests
{
    public class UnitUpgradeShopTests
    {
        [Test]
        public void 유닛_업그레이드_상점은_서로_다른_상품_3개를_제공함()
        {
            var selector = new UnitUpgradeGoodsSelector();

            var result = selector.SelectGoods();

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(3, result.Distinct().Count());
        }

        [Test]
        public void 상품이_랜덤하게_고루_뽑히는지_확인()
        {
            var selector = new UnitUpgradeGoodsSelector();
            var counter = new Dictionary<UnitUpgradeGoods, int>();
            int selectCount = 3333;

            for (int i = 0; i < selectCount; i++)
            {
                foreach (var goods in selector.SelectGoods())
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

            // 확률 분포 확인
            float expectRate = 1f / allGoodsCount;
            float delta = 0.01f;
            foreach (var item in counter)
            {
                float selectRate = item.Value / 3f / (float)selectCount;
                Assert.IsTrue(Mathf.Abs(selectRate - expectRate) < delta);
            }
        }

        [Test]
        public void 입력한_상품은_반환하면_안됨()
        {
            var selector = new UnitUpgradeGoodsSelector();

            var result =  selector.SelectGoodsExcluding(unitGoods);
            // 입력한 상품은 반환하면 안됨
        }
    }
}
