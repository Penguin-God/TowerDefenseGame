using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCombine : TutorialGuideTrigger
{
    int combineCount = 3;

    public override bool TutorialCondition()
    {
        GameObject[] reds = GameObject.FindGameObjectsWithTag("RedSwordman");
        GameObject[] blues = GameObject.FindGameObjectsWithTag("BlueSwordman");
        GameObject[] yellows = GameObject.FindGameObjectsWithTag("YellowSwordman");
        bool condition = (reds.Length >= combineCount || blues.Length >= combineCount || yellows.Length >= combineCount);

        if (condition)
        {
            if (reds.Length >= combineCount) SetCombineNumber(0);
            else if (blues.Length >= combineCount) SetCombineNumber(1);
            else if (yellows.Length >= combineCount) SetCombineNumber(2);
        }

        return condition;
    }

    [SerializeField] Transform tf_CombineColorClick = null;
    [SerializeField] Transform tf_CombineClick = null;
    [SerializeField] RectTransform[] colorButtons = null;
    [SerializeField] RectTransform[] combinButtons = null;
    void SetCombineNumber(int colorNum)
    {
        tf_CombineColorClick.GetComponent<ClickTutorialButton>().SetTutorialUI(colorButtons[colorNum]);
        tf_CombineClick.GetComponent<ClickTutorialButton>().SetTutorialUI(combinButtons[colorNum]);
    }
}