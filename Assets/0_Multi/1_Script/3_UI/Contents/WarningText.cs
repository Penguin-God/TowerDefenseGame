using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : Multi_UI_Popup
{
    Text unitOverText;
    [SerializeField] Color textColor;
    [SerializeField] float showTime;
    protected override void Init()
    {
        base.Init();
        unitOverText = GetComponent<Text>();
        unitOverText.color = textColor;
        gameObject.SetActive(false);
    }

    public void Show(string text)
    {
        StopAllCoroutines();
        GetComponent<Text>().text = text;
        gameObject.SetActive(true);
        StartCoroutine(Co_AfterInActive());
    }

    IEnumerator Co_AfterInActive()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }
}
