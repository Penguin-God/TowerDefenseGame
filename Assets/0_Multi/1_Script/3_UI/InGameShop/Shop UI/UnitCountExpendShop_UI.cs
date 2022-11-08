using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountExpendShop_UI : Multi_UI_Popup
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

        GetButton((int)Buttons.IncreaseButton).onClick.AddListener(IncreaseUnitCount);
        var record = Multi_GameManager.instance.BattleData.MaxUnitIncreaseRecord;
        GetText((int)Texts.PriceText).text = record.GetPriceDescription();
    }

    void IncreaseUnitCount()
    {
        var manager = Multi_GameManager.instance;
        if (manager.TryUseCurrency(manager.BattleData.MaxUnitIncreaseRecord.CurrencyType, manager.BattleData.MaxUnitIncreaseRecord.Price))
            manager.BattleData.MaxUnit += 1;
    }
}
