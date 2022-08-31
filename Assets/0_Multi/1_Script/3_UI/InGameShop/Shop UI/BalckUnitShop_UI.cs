using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalckUnitShop_UI : Multi_UI_Popup
{
    enum Buttons
    {
        BlackSwordmanCombineButton,
        BlackArcherCombineButton,
        BlackSpearmanCombineButton,
        BlackMageCombineButton,
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.BlackSwordmanCombineButton).onClick.AddListener(() => TryCombineBlackUnit(0));
        GetButton((int)Buttons.BlackArcherCombineButton).onClick.AddListener(() => TryCombineBlackUnit(1));
        GetButton((int)Buttons.BlackSpearmanCombineButton).onClick.AddListener(() => TryCombineBlackUnit(2));
        GetButton((int)Buttons.BlackMageCombineButton).onClick.AddListener(() => TryCombineBlackUnit(3));
    }

    void TryCombineBlackUnit(int classNumber)
    {
        if(Multi_UnitManager.Instance.TryCombine_RPC(new UnitFlags(7, classNumber)))
            Multi_Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
    }
}
