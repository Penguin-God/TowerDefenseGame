using System.Collections;
using UnityEngine;
using TMPro;

public class UI_PopupText : UI_Base
{
    TextMeshProUGUI _text;
    
    protected override void Init()
    {
        base.Init();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.raycastTarget = false;
    }

    // 레거시
    public void ShowText(string text, Color color) => ShowText(text, color, new Vector2(0, 50f));

    public void ShowText(string text, Color32 textColor, Vector2 position)
    {
        CheckInit();
        _text.text = text;
        _text.color = textColor;
        _text.GetComponent<RectTransform>().localPosition = position;
    }
}
