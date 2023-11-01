using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShowAndHideController : MonoBehaviour
{
    UI_Manager _uiManager;
    UI_PopupText _textUI;
    const float TextShowTime = 2f;
    readonly Color32 TextColor = new Color32(12, 9, 9, 255);
    readonly Vector2 TextPosition = new Vector2(0, 50);
    void Awake()
    {
        _textUI = Managers.UI.ShowDefualtUI<UI_PopupText>();
        _textUI.gameObject.SetActive(false);
    }

    public void ShowText(string text, Color textColor) => ShowText(text, textColor, TextPosition);
    public void ShowTextForTime(string text) => ShowTextForTime(text, TextPosition);
    public void ShowTextForTime(string text, Vector2 position) => ShowTextForTime(text, TextColor, position);
    public void ShowTextForTime(string text, Color textColor) => ShowTextForTime(text, textColor, TextPosition);

    void ShowText(string text, Color textColor, Vector2 textPosition)
    {
        Managers.UI.SetSotingOrder(_textUI.GetComponent<Canvas>());
        _textUI.gameObject.SetActive(true);
        _textUI.ShowText(text, textColor, textPosition);
    }

    void ShowTextForTime(string text, Color textColor, Vector2 textPosition)
    {
        StopAllCoroutines();
        ShowText(text, textColor, textPosition);
        StartCoroutine(Co_AfterInActive(TextShowTime));
    }

    IEnumerator Co_AfterInActive(float showTime)
    {
        yield return new WaitForSeconds(showTime);
        _textUI.gameObject.SetActive(false);
    }
}
