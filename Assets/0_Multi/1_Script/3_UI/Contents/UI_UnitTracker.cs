using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTracker : Multi_UI_Base
{
    [SerializeField] Image backGround;
    [SerializeField] Image icon;
    [SerializeField] Text countText;

    [SerializeField] UnitNumber unitNumber;

    void Awake()
    {
        backGround = GetComponent<Image>();
    }

    protected override void Init()
    {
        
    }

    public void SetInfo(UI_UnitTrackerData data)
    {
        backGround.color = data.BackGroundColor;
        //icon.sprite = data.Icon;
    }
}
