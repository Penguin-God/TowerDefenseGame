using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupText : UI_Popup
{
    Text _text;
    
    protected override void Init()
    {
        base.Init();
        _text = GetComponent<Text>();
        _text.raycastTarget = false;
        gameObject.SetActive(false);
    }

    public void Show(string text, float showTime) => Show(text, showTime, new Color32(12, 9, 9, 255));

    public void Show(string text, float showTime, Color32 textColor)
    {
        if(_initDone == false)
        {
            Init();
            _initDone = true;
        }

        StopAllCoroutines();
        _text.color = textColor;
        _text.text = text;
        gameObject.SetActive(true);
        StartCoroutine(Co_AfterInActive(showTime));
    }

    IEnumerator Co_AfterInActive(float showTime)
    {
        yield return new WaitForSeconds(showTime);
        Managers.UI.ClosePopupUI();
    }
}
