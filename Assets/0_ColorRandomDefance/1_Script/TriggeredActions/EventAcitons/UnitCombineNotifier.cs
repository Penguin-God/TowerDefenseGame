using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombineNotifier
{
    readonly TextShowAndHideController _textController;
    public UnitCombineNotifier(UnitManager unitManager, TextShowAndHideController textController)
    {
        _textController = textController;
        unitManager.OnCombine += ShowCombineSuccessText;
        unitManager.OnFailedCombine += ShowCombineFaliedText;
    }
    
    void ShowText(string text) => _textController.ShowTextForTime(text);
    void ShowCombineSuccessText(UnitFlags flag) => ShowText($"{UnitTextPresenter.GetUnitNameWithColor(flag)} 조합 성공!!");

    const string FailedText = "조합에 필요한 재료가 부족합니다";
    void ShowCombineFaliedText() => ShowText(FailedText);
}
