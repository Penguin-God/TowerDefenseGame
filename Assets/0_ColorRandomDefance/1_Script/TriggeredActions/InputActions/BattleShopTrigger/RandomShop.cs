using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShop : ShopObject
{
    protected override void ShowShop()
    {
        Managers.UI.ShowPopupUI<UI_UnitUpgradeShop>(path).gameObject.SetActive(true);
        Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
