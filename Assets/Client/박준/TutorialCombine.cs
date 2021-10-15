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

        if (condition) GetComponent<SetCurrentUnitButton>().SetTutorialUI();
        return condition;
    }
}