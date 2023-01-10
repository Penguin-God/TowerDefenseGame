using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class PresentersTester
{
    public void TestGenerateColorChangeResultText()
    {
        Log("색깔 변경 프레젠터 테스트!!");
        var presenter = new UnitColorChangeTextPresenter();
        var beforeFlag = new UnitFlags(1, 2);
        var afterFlag = new UnitFlags(0, 2);
        Assert(presenter.GenerateColorChangeResultText(beforeFlag, afterFlag) == "파란 창병이 빨간 창병으로 변경되었습니다");
        Assert(presenter.GenerateTextShowToDisruptor(beforeFlag, afterFlag) == "스킬 사용으로 상대방의\n파란 창병이 빨간 창병으로 변경되었습니다");
        Assert(presenter.GenerateTextShowToVictim(beforeFlag, afterFlag) == "상대방의 스킬 사용으로 보유 중인\n파란 창병이 빨간 창병으로 변경되었습니다");
    }
}
