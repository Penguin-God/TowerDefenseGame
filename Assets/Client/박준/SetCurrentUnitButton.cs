using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct TutorialUI
{
    public RectTransform[] arr_TutorialUI;
}

public class SetCurrentUnitButton : MonoBehaviour
{
    [SerializeField] int unitCount_MinCondition = 0;
    [SerializeField] ClickTutorialButton[] arr_TutorialButton = null;
    [SerializeField] TutorialUI[] arr_TutorialUI = null;

    private void Start()
    {
        SetTutorialUI();
    }

    public void SetTutorialUI()
    {
        GameObject[] reds = GameObject.FindGameObjectsWithTag("RedSwordman");
        GameObject[] blues = GameObject.FindGameObjectsWithTag("BlueSwordman");
        GameObject[] yellows = GameObject.FindGameObjectsWithTag("YellowSwordman");
        GameObject[] greens = GameObject.FindGameObjectsWithTag("GreenSwordman");
        GameObject[] oranges = GameObject.FindGameObjectsWithTag("OrangeSwordman");
        GameObject[] violets = GameObject.FindGameObjectsWithTag("VioletSwordman");

        if (reds.Length >= unitCount_MinCondition) Set_TutorialUI(0);
        else if (blues.Length >= unitCount_MinCondition) Set_TutorialUI(1);
        else if (yellows.Length >= unitCount_MinCondition) Set_TutorialUI(2);
        else if (greens.Length >= unitCount_MinCondition) Set_TutorialUI(3);
        else if (oranges.Length >= unitCount_MinCondition) Set_TutorialUI(4);
        else if (violets.Length >= unitCount_MinCondition) Set_TutorialUI(5);

    }

    void Set_TutorialUI(int colorNum)
    {
        for(int i = 0; i < arr_TutorialButton.Length; i++)
        {
            RectTransform rect = arr_TutorialUI[i].arr_TutorialUI[colorNum];
            arr_TutorialButton[i].SetTutorialUI(rect);
        }
    }
}
