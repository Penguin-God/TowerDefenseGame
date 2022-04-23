using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellUnitReinForce : MonoBehaviour, ISellEventShopItem
{
    [SerializeField] MyEventType reinforceType;
    [SerializeField] UnitColor ReinforceUnitColor;

    System.Action UnitReinforce = null;

    private void Awake()
    {
        UnitReinforce = EventManager.instance.GetEvent(reinforceType, ReinforceUnitColor);
    }

    //void SetReinForce()
    //{
    //    switch (reinforceType)
    //    {
    //        case MyEventType.Up_UnitDamage:
    //            del_ReinForceUnit = () => EventManager.instance.Up_UnitDamage((int)ReinforceUnitColor); break;
    //        case MyEventType.Up_UnitBossDamage:
    //            del_ReinForceUnit = () => EventManager.instance.Up_UnitBossDamage((int)ReinforceUnitColor); break;
    //        default: Debug.LogWarning("지정하지 않은 이벤트 타입"); break;
    //    }
    //}

    public void Sell_Item()
    {
        if (UnitReinforce != null) UnitReinforce();
    }
}