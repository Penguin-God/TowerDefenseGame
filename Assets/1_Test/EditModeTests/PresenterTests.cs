using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PresenterTests
    {
        [Test]
        [TestCase(GameCurrencyType.Gold, "골드 10원")]
        [TestCase(GameCurrencyType.Food, "고기 10개")]
        public void 재화_종류에_따라_텍스트_생성(GameCurrencyType currencyType, string expected)
        {
            var sut = new GameCurrencyPresenter();

            string result = sut.BuildCurrencyText(currencyType, 10);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void 골드_텍스트_생성()
        {
            var sut = new GameCurrencyPresenter();
            
            string result = sut.BuildGoldText(10);

            Assert.AreEqual("골드 10원", result);
        }

        [Test]
        public void 고기_텍스트_생성()
        {
            var sut = new GameCurrencyPresenter();

            string result = sut.BuildFoodText(10);

            Assert.AreEqual("고기 10개", result);
        }
    }
}
