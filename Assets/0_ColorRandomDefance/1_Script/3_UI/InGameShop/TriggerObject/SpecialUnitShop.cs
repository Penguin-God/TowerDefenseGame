using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUnitShop : ShopObject
{
    protected override void ShowShop()
    {
        Managers.UI.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Popup>(path);
        Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }
}
