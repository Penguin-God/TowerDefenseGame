using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RandomShopPanel_UI : Multi_UI_Base
{
    [SerializeField] Button sellButton;
    [SerializeField] Text text;

    public void Setup(UI_RandomShopGoodsData data, GameObject goods)
    {
        sellButton.onClick.AddListener(() => Sell(data, goods));
        SetText(data);
        gameObject.SetActive(true);
    }

    void Sell(UI_RandomShopGoodsData data, GameObject goods)
    {
        if (Multi_GameManager.instance.TryUseCurrency(data.CurrencyType, data.Price))
        {
            new SellMethodFactory().GetSellMeghod(data.SellType)?.Invoke(data.SellDatas);
            goods.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            // 경고
        }
    }

    void SetText(UI_RandomShopGoodsData data) => text.text = $"{data.Infomation} 구매하시겠습니까?";
}
