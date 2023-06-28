using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineResultText : UI_Popup
{
    [SerializeField] float showTime = 2f;
    [SerializeField] string failedText;
    Text resultText;
    WaitForSeconds waitTime;
    protected override void Init()
    {
        base.Init();
        failedText = "재료가 부족합니다";
        resultText = GetComponent<Text>();
        resultText.text = "";
        waitTime = new WaitForSeconds(showTime);
        Managers.Unit.OnCombine += ShowCombineResultText;
        Managers.Unit.OnFailedCombine += ShowCombineFaliedText;
        gameObject.SetActive(false);
    }

    void ShowCombineResultText(UnitFlags flag)
        => ShowText($"{Managers.Data.UnitNameDataByFlag[flag].KoearName} 조합 성공!!");

    void ShowCombineFaliedText() => ShowText(failedText);

    void ShowText(string text)
    {
        StopAllCoroutines();
        resultText.text = text;
        gameObject.SetActive(true);
        StartCoroutine(Co_AfterInActive());
    }

    IEnumerator Co_AfterInActive()
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }
}
