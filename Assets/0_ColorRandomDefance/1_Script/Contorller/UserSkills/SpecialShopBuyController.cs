using System;
using System.Collections;
using System.Collections.Generic;

public class SpecialShopBuyController : GoodsBuyController
{
    public SpecialShopBuyController(Multi_GameManager gameManager, TextShowAndHideController textController) 
        : base(gameManager, textController) {}

    public override void TryBuy(string qustionText, CurrencyData priceData, Action successAct)
        => base.ShowBuyWindow(qustionText, priceData, successAct);
}
