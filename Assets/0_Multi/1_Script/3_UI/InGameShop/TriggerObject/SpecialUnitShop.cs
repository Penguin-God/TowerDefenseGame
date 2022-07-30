using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUnitShop : ShopObject
{
    protected override void ShowShop()
    {
        Multi_Managers.UI.ShowPopupUI<Multi_UI_Popup>(path, PopupGroupType.UnitWindow);
    }
}
