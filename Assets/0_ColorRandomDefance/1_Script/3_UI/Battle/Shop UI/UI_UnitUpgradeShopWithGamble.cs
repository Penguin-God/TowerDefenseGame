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
    int _gambleLevel;
    int NeedStackForGamble;

    protected override void Init()
    {
        base.Init();
        NeedStackForGamble = 10;
        _gambleLevel = 1;
        _buyController.OnBuyGoods += _ => AddStack();
        Bind<GameObject>(typeof(GameObjects));
    }

    void AddStack()
    {
        _goodsBuyStack++;
        if(_goodsBuyStack >= NeedStackForGamble)
        {
            ConfigureGamble();
            _goodsBuyStack = 0;
            _gambleLevel++;
        }
    }

    void ConfigureGamble()
    {
        // UI
        GetObject((int)GameObjects.Goods).SetActive(true);
        GetObject((int)GameObjects.GamblePanel).SetActive(true);
    }
}
