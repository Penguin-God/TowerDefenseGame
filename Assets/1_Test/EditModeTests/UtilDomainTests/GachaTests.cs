using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UtilDomainTests
{
    public class GachaTests
    {
        [Test]
        public void 지정된_확률대로_뽑혀야_함()
        {
            var gacha = new GachaMachine();
            double[] rates = { 30, 40, 30 };
            int[] selcetCounts = new int[rates.Length];

            int tryCount = 10000;
            for (int i = 0; i < tryCount; i++)
                selcetCounts[gacha.SelectIndex(rates)]++;

            for (int i = 0; i < rates.Length; i++)
            {
                double actualRate = (double)selcetCounts[i] / tryCount * 100;
                double expectedRate = rates[i];

                double rateDelta = 2;

                Assert.IsTrue(rateDelta > Math.Abs(expectedRate - actualRate));
            }
        }


        [Test]
        [TestCase(30, 40, 30.0001)]
        [TestCase(30, 40, 29.9999)]
        public void 확률의_합계가_100이_아니면_예외를_던져야_함(double first, double second, double third)
        {
            var gacha = new GachaMachine();
            double[] rates = { first, second, third };

            Assert.Throws<ArgumentException>(() => gacha.SelectIndex(rates));
        }
    }
}
