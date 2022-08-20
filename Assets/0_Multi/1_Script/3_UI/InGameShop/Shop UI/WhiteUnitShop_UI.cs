using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitShop_UI : Multi_UI_Popup
{
    enum Buttons
    {
        WhiteSwordmanButton,
        WhiteArcherButton,
        WhiteSpearmanButton,
        WhiteMageButton,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        // TODO : 하드코딩 csv로 옮기기
        GetButton((int)Buttons.WhiteSwordmanButton).onClick.AddListener(() => SpawnWhiteUnit(0, 1));
        GetButton((int)Buttons.WhiteArcherButton).onClick.AddListener(() => SpawnWhiteUnit(1, 2));
        GetButton((int)Buttons.WhiteSpearmanButton).onClick.AddListener(() => SpawnWhiteUnit(2, 7));
        GetButton((int)Buttons.WhiteMageButton).onClick.AddListener(() => SpawnWhiteUnit(3, 20));
    }

    void SpawnWhiteUnit(int classNumber, int price)
    {
        if (Multi_GameManager.instance.TryUseFood(price))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(6, classNumber);
            Multi_Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
        }
    }
}
