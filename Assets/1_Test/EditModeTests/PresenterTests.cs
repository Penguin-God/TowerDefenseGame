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

        [Test]
        public void 하얀_유닛_변경_시_나오는_텍스트_생성()
        {
            var sut = new UnitColorChangeTextPresenter();
            var beforeFlag = new UnitFlags(1, 2);
            var afterFlag = new UnitFlags(0, 2);

            Assert.AreEqual("파란 창병이 빨간 창병으로 변경되었습니다", sut.GenerateColorChangeResultText(beforeFlag, afterFlag));

            var beforeFlag2 = new UnitFlags(1, 1);
            var afterFlag2 = new UnitFlags(0, 1);
            Assert.AreEqual("파란 궁수가 빨간 궁수로 변경되었습니다", sut.GenerateColorChangeResultText(beforeFlag2, afterFlag2));

            Assert.AreEqual("스킬 사용으로 상대방의\n파란 창병이 빨간 창병으로 변경되었습니다", sut.GenerateChangerText(beforeFlag, afterFlag));
            Assert.AreEqual("상대방의 스킬 사용으로 보유 중인\n파란 창병이 빨간 창병으로 변경되었습니다", sut.GenerateAffectedText(beforeFlag, afterFlag));
        }

        [Test]
        public void 상대_색깔_변경_시_나오는_텍스트_생성()
        {
            var sut = new UnitColorChangeTextPresenter();
            var beforeFlag = new UnitFlags(1, 2);
            var afterFlag = new UnitFlags(0, 2);

            Assert.AreEqual("스킬 사용으로 상대방의\n파란 창병이 빨간 창병으로 변경되었습니다", sut.GenerateChangerText(beforeFlag, afterFlag));
            Assert.AreEqual("상대방의 스킬 사용으로 보유 중인\n파란 창병이 빨간 창병으로 변경되었습니다", sut.GenerateAffectedText(beforeFlag, afterFlag));
        }
    }
}
