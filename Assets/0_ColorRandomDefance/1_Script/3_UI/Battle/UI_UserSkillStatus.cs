using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UserSkillStatus : UI_Scene
{
    TextMeshProUGUI _text;
    TextMeshProUGUI GetText()
    {
        if(_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
        return _text;
    }

    public void UpdateText(int number) => UpdateText(number.ToString());
    public void UpdateText(string text) => GetText().text = text;
}
