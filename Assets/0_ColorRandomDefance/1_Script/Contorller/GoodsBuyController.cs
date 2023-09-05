using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsBuyController
{
    readonly Multi_GameManager _gameManager;
    readonly TextShowAndHideController _textController;

    public GoodsBuyController(Multi_GameManager gameManager, TextShowAndHideController textController)
    {
        _gameManager = gameManager;
        _textController = textController;
    }

    public void ShowBuyWindow(string qustionText, CurrencyData priceData, Action successAct) 
        => Managers.UI.ShowPopupUI<UI_ComfirmPopup>("UI_ComfirmPopup2").SetInfo(qustionText, () => DoBuy(priceData, successAct));

    void DoBuy(CurrencyData priceData, Action successAct)
    {
        if (_gameManager.TryUseCurrency(priceData))
        {
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            successAct?.Invoke();
        }
        else
        {
            _textController.ShowTextForTime($"{new GameCurrencyPresenter().BuildCurrencyTextWithEnd(priceData.CurrencyType)} 부족해 구매할 수 없습니다.", Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
    }
}
