using System.Collections;
using System.Collections.Generic;

public class UnitCombineNotifier
{
    readonly TextShowAndHideController _textController;
    public UnitCombineNotifier(UnitControllerManager unitManager, TextShowAndHideController textController)
    {
        _textController = textController;
        unitManager.OnCombine += ShowCombineSuccessText;
        unitManager.OnFailedCombine += ShowCombineFaliedText;
    }
    
    void ShowText(string text) => _textController.ShowTextForTime(text);
    void ShowCombineSuccessText(UnitFlags flag) => ShowText($"{UnitTextPresenter.GetUnitNameWithColor(flag)} ���� ����!!");

    const string FailedText = "���տ� �ʿ��� ��ᰡ �����մϴ�";
    void ShowCombineFaliedText() => ShowText(FailedText);
}
