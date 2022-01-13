using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReinForceType
{
    DamageUp,
    BossDamageUp,
}

public class SellUnitReinForce : MonoBehaviour, ISellEventShopItem
{
    public ReinForceType reinForceType;
    [SerializeField] int colorNumber;

    delegate void ReinForceUnit();
    ReinForceUnit reinForceUnit = null;

    private void Awake()
    {
        SetReinForce();
    }

    void SetReinForce()
    {
        switch (reinForceType)
        {
            case ReinForceType.DamageUp:
                reinForceUnit = () => EventManager.instance.Up_UnitDamage(colorNumber);
                break;
            case ReinForceType.BossDamageUp:
                reinForceUnit = () => EventManager.instance.Up_UnitBossDamage(colorNumber);
                break;
        }
    }

    public void Sell_Item()
    {
        if (reinForceUnit != null) reinForceUnit();
    }

}