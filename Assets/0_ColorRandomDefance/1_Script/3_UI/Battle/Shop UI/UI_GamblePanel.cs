using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamblePanel : UI_Base
{
    enum Buttons
    {
        GachaButton
    }

    protected override void Init()
    {
        Bind<Button>(typeof(Buttons));
    }

    int _gambleLevel = 1;
    public event Action OnGamble = null;

    public void SetupGamblePanel()
    {
        double[] rates = new double[] { 25, 25, 25, 25 };
        for (int i = 0; i < rates.Length; i++)
            Managers.UI.MakeSubItem<UI_UnitGachaItemInfo>(transform).ShowInfo((UnitClass)i, rates[i]);
        
        GetButton((int)Buttons.GachaButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.GachaButton).onClick.AddListener(() => UnitGacha(rates));
    }

    void UnitGacha(double[] rates)
    {
        UnitFlags selectUnitFlag = new UnitFlags(UnitFlags.NormalColors.ToList().GetRandom(), (UnitClass)new GachaMachine().SelectIndex(rates));
        Multi_SpawnManagers.NormalUnit.Spawn(selectUnitFlag);
        OnGamble?.Invoke();
        _gambleLevel++;
        GetButton((int)Buttons.GachaButton).onClick.RemoveAllListeners();
    }
}
