using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupText : UI_Popup
{
    Text _text;
    
    protected override void Init()
    {
        base.Init();
        _text = GetComponentInChildren<Text>();
        _text.raycastTarget = false;
    }

    public void Show(string text, float showTime) => Show(text, showTime, new Color32(12, 9, 9, 255));

    public void Show(string text, float showTime, Color32 textColor)
    {
        CheckInit();

        StopAllCoroutines();
        _text.color = textColor;
        _text.text = text;
        StartCoroutine(Co_AfterDestory(showTime));
    }

    public void SetPosition(Vector2 pos)
    {
        CheckInit();
        _text.GetComponent<RectTransform>().localPosition = pos;
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
