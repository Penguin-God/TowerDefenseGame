using System.Collections;
using System.Collections.Generic;

public class UnitCombineNotifier
{
    readonly TextShowAndHideController _textController;
    public UnitCombineNotifier(TextShowAndHideController textController) => _textController = textController;

    void ShowText(string text) => _textController.ShowTextForTime(text);
    public void ShowCombineSuccessText(UnitFlags flag) => ShowText($"{UnitTextPresenter.GetUnitNameWithColor(flag)} ���� ����!!");

    const string FailedText = "���տ� �ʿ��� ��ᰡ �����մϴ�";
    public void ShowCombineFaliedText() => ShowText(FailedText);
}
