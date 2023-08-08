using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitShop_UI : UI_Popup
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
            var unitPriceData = Multi_GameManager.Instance.BattleData.WhiteUnitShopPriceDatas[unitClassNumber];
            GetButton(unitClassNumber).onClick.AddListener(() => SpawnWhiteUnit(unitClassNumber, unitPriceData));
            GetText(unitClassNumber).text = GetPriceText(unitClassNumber, unitPriceData);
        }
    }

    readonly int WHITE_COLOR_NUMBER = 6;
    void SpawnWhiteUnit(int classNumber, CurrencyData data)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(data.CurrencyType, data.Amount))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(WHITE_COLOR_NUMBER, classNumber);
            Managers.UI.ClosePopupUI();
        }
    }

    string GetPriceText(int classNumber, CurrencyData data)
        => $"{UnitTextPresenter.GetUnitName(new UnitFlags(WHITE_COLOR_NUMBER, classNumber))} : {new GameCurrencyPresenter().BuildCurrencyText(data)}";
}
