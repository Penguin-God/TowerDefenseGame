using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WhiteUnitShop : UI_Popup
{
    enum Buttons
    {
        WhiteSwordmanButton,
        WhiteArcherButton,
        WhiteSpearmanButton,
        WhiteMageButton,
    }

    enum Texts
    {
        sowrdmanPrice,
        archerPrice,
        spearmanPrice,
        magePrice,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        foreach (int unitClassNumber in System.Enum.GetValues(typeof(UnitClass)))
        {
            GetButton(unitClassNumber).onClick.AddListener(() => SpawnWhiteUnit(unitClassNumber));
            GetText(unitClassNumber).text = GetPriceText(unitClassNumber);
        }
    }

    ShopDataContainer _shopDataContainer;
    Multi_NormalUnitSpawner _normalUnitSpawner;
    public void DependencyInject(ShopDataContainer shopDataContainer, Multi_NormalUnitSpawner unitSpawner)
    {
        _shopDataContainer = shopDataContainer;
        _normalUnitSpawner = unitSpawner;
    }

    readonly int WHITE_COLOR_NUMBER = (int)UnitColor.White;
    void SpawnWhiteUnit(int classNumber)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(_shopDataContainer.WhiteUnitPriceDatas[classNumber]))
        {
            _normalUnitSpawner.Spawn(WHITE_COLOR_NUMBER, classNumber);
            Managers.UI.ClosePopupUI();
        }
    }

    string GetPriceText(int classNumber)
        => $"{UnitTextPresenter.GetUnitName(new UnitFlags(WHITE_COLOR_NUMBER, classNumber))} : {new GameCurrencyPresenter().BuildCurrencyText(_shopDataContainer.WhiteUnitPriceDatas[classNumber])}";
}
