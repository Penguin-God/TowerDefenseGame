using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountExpendShop_UI : UI_Popup
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

        var priceData = Multi_GameManager.Instance.BattleData.MaxUnitIncreasePriceData;
        GetText((int)Texts.PriceText).text = new GameCurrencyPresenter().BuildCurrencyText(priceData);
        GetButton((int)Buttons.IncreaseButton).onClick.AddListener(() => IncreaseUnitCount(priceData));
    }

    void IncreaseUnitCount(CurrencyData data)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(data.CurrencyType, data.Amount))
            Multi_GameManager.Instance.BattleData.MaxUnit += 1;
    }
}
