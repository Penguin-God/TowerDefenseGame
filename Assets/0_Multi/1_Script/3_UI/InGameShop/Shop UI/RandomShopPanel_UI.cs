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

    public void Setup(UI_RandomShopGoodsData data, GameObject goods)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => Sell(data, goods));
        SetText(data);
        gameObject.SetActive(true);
    }

    void Sell(UI_RandomShopGoodsData data, GameObject goods)
    {
        if (Multi_GameManager.instance.TryUseCurrency(data.CurrencyType, data.Price))
        {
            new SellMethodFactory().GetSellMeghod(data.SellType)?.Invoke(data.SellDatas);
            goods.SetActive(false);
            goods.GetComponent<Button>().onClick.RemoveAllListeners();
            OnSell?.Invoke(goods.GetComponent<Goods_UI>());
            gameObject.SetActive(false);
        }
        else
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show($"{GetCurrcneyTypeText(data.CurrencyType)}가 부족해 구매할 수 없습니다.");
    }

    void Sell(UnityAction sellAct, int price, string currentType)
    {
        if (Multi_GameManager.instance.TryUseCurrency(currentType, price))
        {
            sellAct?.Invoke();
            gameObject.SetActive(false);
        }
        else
            Multi_Managers.UI.ShowPopupUI<WarningText>().Show($"{GetCurrcneyTypeText(currentType)}가 부족해 구매할 수 없습니다.");
    }
    string GetCurrcneyTypeText(string currencyType) => currencyType == "Gold" ? "골드" : "고기";
    void SetText(UI_RandomShopGoodsData data) => text.text = $"{data.Infomation} 구매하시겠습니까?";
}
