using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentUnit : MonoBehaviour
{
    [SerializeField] RectTransform red;
    [SerializeField] RectTransform blue;
    [SerializeField] RectTransform yellow;
    [SerializeField] RectTransform green;
    [SerializeField] RectTransform orange;
    [SerializeField] RectTransform violet;

    [SerializeField] List<RectTransform> buttonPositonList = new List<RectTransform>();
    [SerializeField] RectTransform[] rectTransforms;
    public List<int> testIntList = new List<int>() { 0, 1, 2, 3, 4, 5, 6};

    private void Awake()
    {
        buttonPositonList.Add(red);
        buttonPositonList.Add(blue);
        buttonPositonList.Add(yellow);
        buttonPositonList.Add(green);
        buttonPositonList.Add(orange);
        buttonPositonList.Add(violet);
    }

    private void Start()
    {
        StartCoroutine(Set_CurrentUnitUI());
    }

    IEnumerator Set_CurrentUnitUI()
    {
        while (true)
        {
            //SetButtons();
            Debug.Log(1);
            yield return new WaitForSeconds(0.2f);
            Debug.Log(2);
            SetButtonList();
            yield return new WaitForSeconds(0.2f);
            Debug.Log(3);
            SetButtonPosition();
            yield return new WaitForSeconds(0.2f);
        }
    }

    //void CheckColor(bool p_Color, RectTransform p_Button)
    //{
    //    if()
    //}

    void SetButtonPosition()
    {
        for(int i = 0; i < buttonPositonList.Count; i++)
        {
            buttonPositonList[i].position = rectTransforms[i].position;
        }
    }

    void SetButtonList()
    {
        for(int i = 0; i < buttonPositonList.Count; i++)
        {
            UnitButtonStatus buttonStatus = buttonPositonList[i].GetComponent<UnitButtonStatus>();
            if (buttonStatus.isShow && !buttonStatus.isFix && i != 0)
            {
                buttonStatus.isFix = true;
                RectTransform rc = buttonPositonList[i]; 
                buttonPositonList.Remove(rc);
                buttonPositonList.Insert(ReturnInsertNum(buttonStatus), rc);
                //break;
            }
        }
    }

    int ReturnInsertNum(UnitButtonStatus buttonStatus)
    {
        Debug.Log(buttonPositonList.Count);
        for (int i = 0; i < buttonPositonList.Count; i++)
        {
            UnitButtonStatus compareStatus = buttonPositonList[i].GetComponent<UnitButtonStatus>();
            if (compareStatus.isShow && compareStatus.priority > buttonStatus.priority) continue;
            else if (compareStatus == buttonStatus) return i;
            else return i;
        }
        return buttonPositonList.Count;
    }

    void SetButton(bool p_IsColor, RectTransform button)
    {
        UnitButtonStatus buttonStatus = button.GetComponent<UnitButtonStatus>();
        if (p_IsColor && buttonStatus != null) buttonStatus.isShow = true;
    }

    //void SetButtons()
    //{
    //    SetButton(UnitManager.instance.thereIs_RedUnit, red);
    //    SetButton(UnitManager.instance.thereIs_BlueUnit, blue);
    //    SetButton(UnitManager.instance.thereIs_YellowUnit, yellow);
    //    SetButton(UnitManager.instance.thereIs_GreenUnit, green);
    //    SetButton(UnitManager.instance.thereIs_OrangeUnit, orange);
    //    SetButton(UnitManager.instance.thereIs_VioletUnit, violet);
    //}
}
