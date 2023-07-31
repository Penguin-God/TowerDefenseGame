using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupText : UI_Base
{
    Text _text;
    const float TextShowTime = 2f;
    readonly Vector2 TextPosition = new Vector2(0, 120f);

    protected override void Init()
    {
        base.Init();
        _text = GetComponentInChildren<Text>();
        _text.raycastTarget = false;
    }

    public void ShowText(string text, Color color) => SetUI(text, color);
    public void ShowTextForTime(string text) => ShowTextForTime(text, new Color32(12, 9, 9, 255));

    public void ShowTextForTime(string text, Color textColor)
    {
        StopAllCoroutines();
        SetUI(text, textColor);
        StartCoroutine(Co_AfterDestory(TextShowTime));
    }

    void SetUI(string text, Color32 textColor)
    {
        CheckInit();
        _text.text = text;
        _text.color = textColor;
        _text.GetComponent<RectTransform>().localPosition = TextPosition;
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
