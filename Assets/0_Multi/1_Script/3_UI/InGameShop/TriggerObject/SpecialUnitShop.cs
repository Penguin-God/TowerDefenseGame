using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUnitShop : ShopObject
{
    protected override void ShowShop()
    {
        Managers.UI.ShowPopGroupUI<UI_Popup>(PopupGroupType.UnitWindow, path);
        Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }
}
