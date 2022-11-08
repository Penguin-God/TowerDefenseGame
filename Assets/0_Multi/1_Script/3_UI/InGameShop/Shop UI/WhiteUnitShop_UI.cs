using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteUnitShop_UI : Multi_UI_Popup
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
            var unitPriceData = Multi_GameManager.instance.BattleData.WhiteUnitPriceRecord.GetData(unitClassNumber);
            GetButton(unitClassNumber).onClick.AddListener(() => SpawnWhiteUnit(unitClassNumber, unitPriceData));
            GetText(unitClassNumber).text = GetPriceText(unitClassNumber, unitPriceData);
        }
    }

    void SpawnWhiteUnit(int classNumber, PriceData record)
    {
        if (Multi_GameManager.instance.TryUseCurrency(record.CurrencyType, record.Price))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(6, classNumber);
            Multi_Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
        }
    }

    string GetPriceText(int classNumber, PriceData record)
        => $"{new UnitFlags(6, classNumber).KoreaName} : {record.GetPriceDescription()}";
}
