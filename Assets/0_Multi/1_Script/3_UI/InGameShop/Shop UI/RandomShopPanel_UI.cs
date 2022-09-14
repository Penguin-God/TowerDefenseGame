using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class RandomShopPanel_UI : Multi_UI_Base
{
    [SerializeField] Button sellButton;
    [SerializeField] Text text;
    public event Action<Goods_UI> OnSell;

    public void Setup(UnityAction sellAct, int price, string currencyType, string _text)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => Sell(sellAct, price, currencyType));
        text.text = _text;
        gameObject.SetActive(true);
    }

    public void Setup(UI_RandomShopGoodsData data, GoodsManager goodsManager, UnityAction sellAct = null)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellAct(data, goodsManager, sellAct));
        SetGoodsInfoText(data);
        gameObject.SetActive(true);
    }

    public void Setup(Goods_UI goods, GoodsManager goodsManager)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellAct(goods, goodsManager));
        SetGoodsInfoText(goodsManager.LocationByData[goods.Loaction]);
        gameObject.SetActive(true);
    }

    void SellAct(Goods_UI goods, GoodsManager goodsManager, UnityAction sellAct = null)
    {
        UI_RandomShopGoodsData data = goodsManager.LocationByData[goods.Loaction];
        bool isSuccess = new GoodsSellUseCase().TrySell(data, goodsManager);

        if (isSuccess)
        {
            Multi_Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            goods.gameObject.SetActive(false);
        }
        else
        {
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show($"{GetCurrcneyTypeText(data.CurrencyType)}가 부족해 구매할 수 없습니다.");
            Multi_Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        gameObject.SetActive(false);
    }

    void SellAct(UI_RandomShopGoodsData data, GoodsManager goodsManager, UnityAction sellAct = null)
    {
        bool isSuccess = new GoodsSellUseCase().TrySell(data, goodsManager);

        if (isSuccess)
        {
            Multi_Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            sellAct?.Invoke();
        }
        else
        {
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show($"{GetCurrcneyTypeText(data.CurrencyType)}가 부족해 구매할 수 없습니다.");
            Multi_Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        gameObject.SetActive(false);
    }

    void Sell(UnityAction sellAct, int price, string currentType)
    {
        if (Multi_GameManager.instance.TryUseCurrency(currentType, price))
        {
            sellAct?.Invoke();
            gameObject.SetActive(false);
            Multi_Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
        }
        else
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show($"{GetCurrcneyTypeText(currentType)}가 부족해 구매할 수 없습니다.");
    }
    string GetCurrcneyTypeText(string currencyType) => currencyType == "Gold" ? "골드" : "고기";
    string GetCurrcneyTypeText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
    void SetGoodsInfoText(UI_RandomShopGoodsData data) => text.text = $"{data.Infomation} 구매하시겠습니까?";
}
