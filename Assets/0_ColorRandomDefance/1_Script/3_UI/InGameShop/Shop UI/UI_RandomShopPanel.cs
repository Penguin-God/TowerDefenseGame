using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_RandomShopPanel : UI_Popup
{
    [SerializeField] Button sellButton;
    [SerializeField] Text buyQuestionText;

    public void Setup(UnitUpgradeGoodsData goodsData, BuyController buyController)
    {
        buyQuestionText.text = BuildQustionText(goodsData);
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => buyController.Buy(goodsData));
        sellButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    string GetCurrcneyTypeText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
    string BuildQustionText(UnitUpgradeGoodsData goodsData)
    {
        var goodsPresenter = new UnitUpgradeGoodsPresenter();
        return $"{goodsPresenter.BuildGoodsText(goodsData.UpgradeGoods)}를 {GetCurrcneyTypeText(goodsData.Currency)} {goodsData.Price}에 구매하시겠습니까?";
    }
}
