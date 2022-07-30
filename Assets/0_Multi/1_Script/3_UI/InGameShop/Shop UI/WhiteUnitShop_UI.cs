using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum Buttons
{
    WhiteSwordmanButton,
    WhiteArcherButton,
    WhiteSpearmanButton,
    WhiteMageButton,
}

public class WhiteUnitShop_UI : Multi_UI_Popup
{
    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.WhiteSwordmanButton).onClick.AddListener(() => SpawnWhiteUnit(0, 1));
        GetButton((int)Buttons.WhiteArcherButton).onClick.AddListener(() => SpawnWhiteUnit(1, 2));
        GetButton((int)Buttons.WhiteSpearmanButton).onClick.AddListener(() => SpawnWhiteUnit(2, 7));
        GetButton((int)Buttons.WhiteMageButton).onClick.AddListener(() => SpawnWhiteUnit(3, 20));
    }

    void SpawnWhiteUnit(int classNumber, int price) // TODO : 하드코딩 csv로 옮기기
    {
        if(Multi_GameManager.instance.TryUseFood(price))
            Multi_SpawnManagers.NormalUnit.Spawn(6, classNumber);
    }
}
