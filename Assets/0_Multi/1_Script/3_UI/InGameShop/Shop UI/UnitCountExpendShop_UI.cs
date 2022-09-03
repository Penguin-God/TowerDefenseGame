using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountExpendShop_UI : Multi_UI_Popup
{
    enum Buttons
    {
        IncreaseButton,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.IncreaseButton).onClick.AddListener(IncreaseUnitCount);
    }

    void IncreaseUnitCount()
    {
        if(Multi_GameManager.instance.TryUseFood(1))
            Multi_GameManager.instance.IncreaseUnitMaxCount();
    }
}
