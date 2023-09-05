using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsBuyController
{
    readonly Multi_GameManager _gameManager;
    readonly BuyAction _buyActionFactory;
    readonly TextShowAndHideController _textController;

    public GoodsBuyController(Multi_GameManager gameManager, BuyAction buyActionFactory, TextShowAndHideController textController)
    {
        _gameManager = gameManager;
        _buyActionFactory = buyActionFactory;
        _textController = textController;
    }

    public bool TryBuy(BattleShopGoodsData data)
    {
        if (_gameManager.TryUseCurrency(data.PriceData))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            _buyActionFactory.Do(data.SellData);
            return true;
        }
        else
        {
            _textController.ShowTextForTime($"{new GameCurrencyPresenter().BuildCurrencyTextWithEnd(data.PriceData.CurrencyType)} 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
            return false;
        }
    }
}

public class BuyAction
{
    readonly Multi_NormalUnitSpawner _unitSpawner;
    readonly UnitUpgradeController _unitUpgradeController;
    public BuyAction(Multi_NormalUnitSpawner unitSpawner, UnitUpgradeController unitUpgradeController)
    {
        _unitSpawner = unitSpawner;
        _unitUpgradeController = unitUpgradeController;
    }

    public void Do(GoodsData sellData)
    {
        var convertor = new DataConvertUtili();
        switch (sellData.GoodsType)
        {
            case BattleShopGoodsType.Unit: SpawnUnit(convertor.ToUnitFlag(sellData.Datas)); break;
            case BattleShopGoodsType.UnitUpgrade: UpgradeUnit(convertor.ToUnitUpgradeData(sellData.Datas)); break;
            default: Debug.LogError($"정의되지 않은 굿즈 타입 {sellData.GoodsType}"); break;
        }
    }

    void SpawnUnit(UnitFlags flag) => _unitSpawner.Spawn(flag);

    void UpgradeUnit(UnitUpgradeData goods)
    {
        const float Percentage = 100f;
        switch (goods.UpgradeType)
        {
            case UnitUpgradeType.Value:
                _unitUpgradeController.AddUnitDamageValue(goods.TargetColor, goods.Value, UnitStatType.All); break;
            case UnitUpgradeType.Scale:
                _unitUpgradeController.ScaleUnitDamageValue(goods.TargetColor, goods.Value / Percentage, UnitStatType.All);
                Multi_GameManager.Instance.IncrementUnitUpgradeValue(goods);
                break;
        }
    }
}


