using System.Collections;
using UnityEngine;
using TMPro;

public class UI_PopupText : UI_Base
{
    TextMeshProUGUI _text;
    const float TextShowTime = 2f;
    readonly Color32 TextColor = new Color32(12, 9, 9, 255);
    readonly Vector2 TextPosition = new Vector2(0, 120f);

    protected override void Init()
    {
        base.Init();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.raycastTarget = false;
    }

    public void ShowText(string text, Color color) => SetUI(text, color, TextPosition);
    public void ShowTextForTime(string text) => ShowTextForTime(text, TextPosition);
    public void ShowTextForTime(string text, Vector2 position) => ShowTextForTime(text, TextColor, position);
    public void ShowTextForTime(string text, Color textColor) => ShowTextForTime(text, textColor, TextPosition);

    void ShowTextForTime(string text, Color textColor, Vector2 position)
    {
        StopAllCoroutines();
        SetUI(text, textColor, position);
        StartCoroutine(Co_AfterDestory(TextShowTime));
    }

    void SetUI(string text, Color32 textColor, Vector2 position)
    {
        CheckInit();
        _text.text = text;
        _text.color = textColor;
        _text.GetComponent<RectTransform>().localPosition = position;
    }

    public void OnRaycastTarget() // 이거는 나중에 배리어 UI를 따로 만들어야 함
    {
        CheckInit();
        _text.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 480);
        _text.raycastTarget = true;
    }

    IEnumerator Co_AfterDestory(float showTime)
    {
        yield return new WaitForSeconds(showTime);
        Destroy(gameObject);
    }
}
