﻿using System.Collections;
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
            var unitPriceData = Multi_GameManager.Instance.BattleData.WhiteUnitPriceRecord.GetData(unitClassNumber);
            GetButton(unitClassNumber).onClick.AddListener(() => SpawnWhiteUnit(unitClassNumber, unitPriceData));
            GetText(unitClassNumber).text = GetPriceText(unitClassNumber, unitPriceData);
        }
    }

    readonly int WHITE_COLOR_NUMBER = 6;
    void SpawnWhiteUnit(int classNumber, PriceData record)
    {
        if (Multi_GameManager.Instance.TryUseCurrency(record.CurrencyType, record.Price))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(WHITE_COLOR_NUMBER, classNumber);
            Managers.UI.ClosePopupUI(PopupGroupType.UnitWindow);
        }
    }

    string GetPriceText(int classNumber, PriceData record)
        => $"{UnitPresenter.GetUnitName(new UnitFlags(WHITE_COLOR_NUMBER, classNumber))} : {record.GetPriceDescription()}";
}
