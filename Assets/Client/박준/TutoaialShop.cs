using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoaialShop : TutorialGuideTrigger
{
    [SerializeField] Shop shop = null;
    //[SerializeField] GameObject ShopObject = null;
    public override bool TutorialCondition()
    {
        return shop.gameObject.activeSelf;
    }
}
