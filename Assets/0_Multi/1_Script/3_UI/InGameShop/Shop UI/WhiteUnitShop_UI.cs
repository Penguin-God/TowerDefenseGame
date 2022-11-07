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

        UnitPriceRecord whiteUnitPriceRecord = Multi_GameManager.instance.BattleData.WhiteUnitPriceRecord;

        foreach (int unitClassNumber in System.Enum.GetValues(typeof(UnitClass)))
        {
            GetButton(unitClassNumber).onClick.AddListener(() => SpawnWhiteUnit(unitClassNumber, whiteUnitPriceRecord));
            GetText(unitClassNumber).text = GetPriceText(unitClassNumber, whiteUnitPriceRecord);
        }
    }

    void SpawnWhiteUnit(int classNumber, UnitPriceRecord record)
    {
        if (Multi_GameManager.instance.TryUseCurrency(record.CurrencyType, record.GetUnitData(classNumber)))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(6, classNumber);
            Multi_Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
        }
    }

    string GetPriceText(int classNumber, UnitPriceRecord record)
        => $"{new UnitFlags(6, classNumber).KoreaName} : {record.GetCurrencyKoreaText()} {record.GetUnitData(classNumber)}{record.GetQuantityInfoText()}";
}
