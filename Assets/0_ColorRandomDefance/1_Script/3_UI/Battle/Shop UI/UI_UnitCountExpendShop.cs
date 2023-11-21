using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitCountExpendShop : UI_Popup
{
    enum Buttons
    {
        IncreaseButton,
    }

    enum Texts
    {
        PriceText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.PriceText).text = new GameCurrencyPresenter().BuildCurrencyText(_shopDataContainer.MaxUnitIncreasePriceData);
        GetButton((int)Buttons.IncreaseButton).onClick.AddListener(IncreaseUnitCount);
    }

    ShopDataContainer _shopDataContainer;
    MultiBattleDataController _battleDataController;
    public void DependencyInject(ShopDataContainer shopDataContainer, MultiBattleDataController battleDataController)
    {
        _shopDataContainer = shopDataContainer;
        _battleDataController = battleDataController;
    }

    const int IncreaseAmount = 1;
    void IncreaseUnitCount()
    {
        if (Multi_GameManager.Instance.TryUseCurrency(_shopDataContainer.MaxUnitIncreasePriceData))
            _battleDataController.IncreasedMaxUnitCount(IncreaseAmount);
    }
}
