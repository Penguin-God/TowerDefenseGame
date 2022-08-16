using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : Multi_UI_Popup
{
    Text waringText;
    [SerializeField] Color textColor;
    [SerializeField] float showTime;
    protected override void Init()
    {
        base.Init();
        waringText = GetComponent<Text>();
        waringText.color = textColor;
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        StopAllCoroutines();
        GetComponent<Text>().text = text;
        gameObject.SetActive(true);
        StartCoroutine(Co_AfterInActive());
    }

    public void ShowClickLockWaringText(string text)
    {
        waringText.raycastTarget = true;
        Show(text);
    }

    IEnumerator Co_AfterInActive()
    {
        yield return new WaitForSeconds(showTime);
        waringText.raycastTarget = false;
        gameObject.SetActive(false);
    }
}
