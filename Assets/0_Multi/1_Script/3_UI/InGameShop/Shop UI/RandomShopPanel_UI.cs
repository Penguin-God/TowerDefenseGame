using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RandomShopPanel_UI : Multi_UI_Base
{
    [SerializeField] Button sellButton;
    [SerializeField] Text text;

    public void Setup(UI_RandomShopGoodsData data, GoodsManager goodsManager, UnityAction sellAct = null)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellAct(data, goodsManager, sellAct));
        SetGoodsInfoText(data);
        gameObject.SetActive(true);
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

    string GetCurrcneyTypeText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
    void SetGoodsInfoText(UI_RandomShopGoodsData data) => text.text = $"{data.Infomation} 구매하시겠습니까?";
}
