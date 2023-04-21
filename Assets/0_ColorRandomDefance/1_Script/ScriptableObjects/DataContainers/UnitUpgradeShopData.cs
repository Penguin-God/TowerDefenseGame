using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/UnitUpgradeShop")]
public class UnitUpgradeShopData : ScriptableObject
{
    public int AddValue;
    public CurrencyData AddValuePriceData;
    public int UpScale;
    public float UpScaleApplyValue => UpScale / 100f;
    public CurrencyData UpScalePriceData;
    public int ResetPrice;
}
