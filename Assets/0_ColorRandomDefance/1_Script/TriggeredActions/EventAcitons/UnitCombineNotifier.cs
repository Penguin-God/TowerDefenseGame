using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombineNotifier
{
    readonly UI_Manager _ui;
    readonly IReadOnlyDictionary<UnitFlags, UnitNameData> _unitNameDataByFlag;
    public UnitCombineNotifier(UI_Manager ui, IReadOnlyDictionary<UnitFlags, UnitNameData> unitNameDataByFlag)
    {
        _ui = ui;
        _unitNameDataByFlag = unitNameDataByFlag;
    }

    public void Init(UnitManager unitManager)
    {
        unitManager.OnCombine += ShowCombineSuccessText;
        unitManager.OnFailedCombine += ShowCombineFaliedText;
    }
    
    void ShowText(string text) => _ui.ShowDefualtUI<UI_PopupText>().ShowTextForTime(text);
    void ShowCombineSuccessText(UnitFlags flag) => ShowText($"{_unitNameDataByFlag[flag].KoearName} ���� ����!!");

    const string FailedText = "���տ� �ʿ��� ��ᰡ �����մϴ�";
    void ShowCombineFaliedText() => ShowText(FailedText);
}
