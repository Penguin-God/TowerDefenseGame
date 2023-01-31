using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitColorChangeTextPresenter
{
    public string ChangeFaildText => "상대방에게 변경 가능한 유닛이 존재하지 않습니다.";
    public string GenerateColorChangeResultText(UnitFlags before, UnitFlags after) 
        => $"{before.KoreaName}이 {after.KoreaName}으로 변경되었습니다";

    public string GenerateTextShowToDisruptor(UnitFlags before, UnitFlags after)
        => $"스킬 사용으로 상대방의\n{GenerateColorChangeResultText(before, after)}";

    public string GenerateTextShowToVictim(UnitFlags before, UnitFlags after)
        => $"상대방의 스킬 사용으로 보유 중인\n{GenerateColorChangeResultText(before, after)}";
}
