using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombineNotifier
{
    readonly UI_Manager _ui;
    public UnitCombineNotifier(UI_Manager ui) => _ui = ui;

    public void Init(UnitManager unitManager)
    {
        unitManager.OnCombine += ShowCombineSuccessText;
        unitManager.OnFailedCombine += ShowCombineFaliedText;
    }
    
    void ShowText(string text) => _ui.ShowDefualtUI<UI_PopupText>().ShowTextForTime(text);
    void ShowCombineSuccessText(UnitFlags flag) => ShowText($"{UnitTextPresenter.GetUnitNameWithColor(flag)} ���� ����!!");

    const string FailedText = "���տ� �ʿ��� ��ᰡ �����մϴ�";
    void ShowCombineFaliedText() => ShowText(FailedText);
}
