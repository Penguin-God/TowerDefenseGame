using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UnitUpgradeShopWithGamble : UI_UnitUpgradeShop
{
    enum GameObjects
    {
        GamblePanel,
        Goods,
    }

    enum Texts
    {
        GambleStackText
    }

    int _goodsBuyStack;
    int NeedStackForGamble;
    UI_GamblePanel _gamblePanel;

    protected override void Init()
    {
        base.Init();
        NeedStackForGamble = 10;
        _buyController.OnBuyGoods += _ => AddStack();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        _gamblePanel = GetComponentInChildren<UI_GamblePanel>(true);
        _gamblePanel.Inject(base._textController);
        _gamblePanel.OnGamble += EndGamble;

        GetTextMeshPro((int)Texts.GambleStackText).text = $"다음 도박까지 구매해야하는 상품 개수 : {NeedStackForGamble}";
    }

    void AddStack()
    {
        _goodsBuyStack++;
        if (_goodsBuyStack >= NeedStackForGamble)
        {
            ConfigureGamble();
            _goodsBuyStack = 0;
        }
        GetTextMeshPro((int)Texts.GambleStackText).text = $"다음 도박까지 구매해야하는 상품 개수 : {NeedStackForGamble - _goodsBuyStack}";
    }

    void ConfigureGamble()
    {
        GetTextMeshPro((int)Texts.GambleStackText).gameObject.SetActive(false);
        GetObject((int)GameObjects.Goods).SetActive(false);
        _gamblePanel.gameObject.SetActive(true);
        _gamblePanel.SetupGamblePanel();
    }

    void EndGamble()
    {
        GetTextMeshPro((int)Texts.GambleStackText).gameObject.SetActive(true);
        GetObject((int)GameObjects.Goods).SetActive(true);
        _gamblePanel.gameObject.SetActive(false);
    }
}
