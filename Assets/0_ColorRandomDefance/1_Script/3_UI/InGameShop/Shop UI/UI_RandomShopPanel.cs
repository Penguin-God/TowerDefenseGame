using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_RandomShopPanel : UI_Popup
{
    [SerializeField] Button sellButton;
    [SerializeField] Text text;

    public void Setup(UnitUpgradeGoods goods, BuyController buyController)
    {
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => print("안타가 키라이나")); // buyController.Buy();
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
    void SetGoodsInfoText(UI_RandomShopGoodsData data) => text.text = $"{data.Infomation} 구매하시겠습니까?";
}
