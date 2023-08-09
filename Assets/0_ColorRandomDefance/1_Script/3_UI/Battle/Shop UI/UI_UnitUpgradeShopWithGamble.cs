using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitUpgradeShopWithGamble : UI_UnitUpgradeShop
{
    enum GameObjects
    {
        GamblePanel,
        Goods,
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
        _gamblePanel = GetComponentInChildren<UI_GamblePanel>(true);
        _gamblePanel.OnGamble += EndGamble;
    }

    void AddStack()
    {
        _goodsBuyStack++;
        if(_goodsBuyStack >= NeedStackForGamble)
        {
            ConfigureGamble();
            _goodsBuyStack = 0;
        }
    }

    void ConfigureGamble()
    {
        GetObject((int)GameObjects.Goods).SetActive(false);
        _gamblePanel.gameObject.SetActive(true);
        _gamblePanel.SetupGamblePanel();
    }

    void EndGamble()
    {
        GetObject((int)GameObjects.Goods).SetActive(true);
        _gamblePanel.gameObject.SetActive(false);
    }
}
