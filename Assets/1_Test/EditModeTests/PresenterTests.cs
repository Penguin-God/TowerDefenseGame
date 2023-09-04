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
        [TestCase(GameCurrencyType.Rune, "룬 10개")]
        public void 재화_종류에_따라_텍스트_생성(GameCurrencyType currencyType, string expected)
        {
            var sut = new GameCurrencyPresenter();
            var data = new CurrencyData(currencyType, 10);
            string result = sut.BuildCurrencyText(data);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void 유닛_색깔_변경_시_나오는_텍스트는_적절한_유닛이름과_HTML_Color_코드가_들어가야_함()
        {
            var sut = new UnitColorChangeTextPresenter();
            var beforeFlag = new UnitFlags(1, 2);
            var afterFlag = new UnitFlags(0, 2);
            Assert.AreEqual("<color=#0000FF>파란 창병</color>이 <color=#FF0000>빨간 창병</color>으로 변경되었습니다", sut.GenerateColorChangeResultText(beforeFlag, afterFlag));

            var beforeFlag2 = new UnitFlags(1, 1);
            var afterFlag2 = new UnitFlags(0, 1);
            Assert.AreEqual("<color=#0000FF>파란 궁수</color>가 <color=#FF0000>빨간 궁수</color>로 변경되었습니다", sut.GenerateColorChangeResultText(beforeFlag2, afterFlag2));
        }
    }
}
