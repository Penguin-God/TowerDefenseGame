using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShop : ShopObject
{
    protected override void ShowShop()
    {
        Multi_Managers.UI.ShowPopupUI<RandomShop_UI>(path).gameObject.SetActive(true);
        Multi_Managers.Sound.PlayEffect(EffectSoundType.ShowRandomShop);
    }
}
