using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUpgradeShopController
{
    public event Action<UnitUpgradeGoodsData> OnBuyGoods;
    UnitUpgradeShopData _unitUpgradeShopData;
    TextShowAndHideController _textController;

    public UnitUpgradeShopController(UnitUpgradeShopData unitUpgradeShopData, TextShowAndHideController textController)
    {
        _unitUpgradeShopData = unitUpgradeShopData;
        _textController = textController;
    }

    public void Buy(UnitUpgradeGoodsData upgradeData)
    {
        var priceData = Multi_GameManager.Instance.BattleData.ShopPriceDataByUnitUpgradeData[upgradeData];
        if (Multi_GameManager.Instance.TryUseCurrency(priceData.CurrencyType, priceData.Amount))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            UpgradeUnit(upgradeData);
            Multi_GameManager.Instance.IncrementUnitUpgradeValue(upgradeData);
            OnBuyGoods?.Invoke(upgradeData);
        }
        else
        {
            _textController.ShowTextForTime($"{new GameCurrencyPresenter().BuildCurrencyTypeText(priceData.CurrencyType)}이 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }

    void UpgradeUnit(UnitUpgradeGoodsData goods)
    {
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                MultiServiceMidiator.UnitUpgrade.AddUnitDamageValue(goods.TargetColor, _unitUpgradeShopData.AddValue, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                MultiServiceMidiator.UnitUpgrade.ScaleUnitDamageValue(goods.TargetColor, _unitUpgradeShopData.ApplyUpScale, UnitStatType.All); break;
        }
    }
}
