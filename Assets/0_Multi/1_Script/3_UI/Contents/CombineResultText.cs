﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineResultText : Multi_UI_Popup
{
    [SerializeField] float showTime = 2f;
    [SerializeField] string failedText = "재료가 부족합니다";
    Text resultText;
    WaitForSeconds waitTime;
    protected override void Init()
    {
        base.Init();
        resultText = GetComponent<Text>();
        resultText.text = "";
        waitTime = new WaitForSeconds(showTime);
        Multi_UnitManager.Instance.OnTryCombine += ShowCombineResult;
        gameObject.SetActive(false);
    }

    void ShowCombineResult(bool isCombineSuccess, UnitFlags flag)
    {
        resultText.text = (isCombineSuccess) ? GetSuccessText() : failedText;
        gameObject.SetActive(true);
        StartCoroutine(Co_AfterInActive());

        string GetSuccessText() => $"{Multi_Managers.Data.CombineDataByUnitFlags[flag].KoearName} 조합 성공!!";
    }

    IEnumerator Co_AfterInActive()
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }
}
