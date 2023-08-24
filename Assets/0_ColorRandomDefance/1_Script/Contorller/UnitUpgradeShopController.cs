using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitUpgradeData
{
    public UnitUpgradeType UpgradeType { get; private set; }
    public UnitColor TargetColor { get; private set; }
    public int Value { get; private set; }
    public CurrencyData PriceData { get; private set; }
    public UnitUpgradeData(UnitUpgradeType upgradeType, UnitColor color, int value, CurrencyData priceData)
    {
        UpgradeType = upgradeType;
        TargetColor = color;
        Value = value;
        PriceData = priceData;
    }
}

public class UnitUpgradeShopController
{
    TextShowAndHideController _textController;
    public UnitUpgradeShopController(TextShowAndHideController textController) => _textController = textController;

    public void Buy(UnitUpgradeData upgradeData)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(upgradeData.PriceData))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            UpgradeUnit(upgradeData);
            Multi_GameManager.Instance.IncrementUnitUpgradeValue(upgradeData);
        }
        else
        {
            _textController.ShowTextForTime($"{new GameCurrencyPresenter().BuildCurrencyTypeText(upgradeData.PriceData.CurrencyType)}이 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }

    void UpgradeUnit(UnitUpgradeData goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(goods.TargetColor, goods.Value / 100f, UnitStatType.All); break;
        }
    }
}
