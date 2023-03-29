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
            Managers.Sound.PlayEffect(EffectSoundType.GoodsBuySound);
            sellAct?.Invoke();
        }
        else
        {
            Managers.UI.ShowDefualtUI<UI_PopupText>().Show($"{GetCurrcneyTypeText(data.CurrencyType)}가 부족해 구매할 수 없습니다.", 2f, Color.red);
            Managers.Sound.PlayEffect(EffectSoundType.Denger);
        }
        gameObject.SetActive(false);
    }

    string GetCurrcneyTypeText(GameCurrencyType type) => type == GameCurrencyType.Gold ? "골드" : "고기";
    void SetGoodsInfoText(UI_RandomShopGoodsData data) => buyQuestionText.text = $"{data.Infomation} 구매하시겠습니까?";
    string BuildQustionText(UnitUpgradeGoodsData goodsData)
    {
        var goodsPresenter = new UnitUpgradeGoodsPresenter();
        return $"{goodsPresenter.BuildGoodsText(goodsData.UpgradeGoods)}를 {GetCurrcneyTypeText(goodsData.Currency)} {goodsData.Price}에 구매하시겠습니까?";
    }
}
