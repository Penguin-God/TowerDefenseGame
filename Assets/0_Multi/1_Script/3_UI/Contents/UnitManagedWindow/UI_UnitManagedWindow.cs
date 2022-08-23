using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Popup
{
    [SerializeField] Text _description;
    [SerializeField] UI_CombineButtonParent _combineButtonsParent;

    [SerializeField] Text _currentWolrd;
    [SerializeField] UnitWolrdChangedButton worldChangedButton;
    [SerializeField] UnitSellButton unitSellButton;

    protected override void Init()
    {
        base.Init();
        //_combineButtonsParent = GetComponentInChildren<UI_CombineButtonParent>();
        //worldChangedButton = GetComponentInChildren<UnitWolrdChangedButton>();
        //unitSellButton = GetComponentInChildren<UnitSellButton>();
    }

    public void Show(UnitFlags flags)
    {
        SetInfo(flags);
        gameObject.SetActive(true);
    }

    void SetInfo(UnitFlags flag)
    {
        _description.text = Multi_Managers.Data.UnitWindowDataByUnitFlags[flag].Description;
        _combineButtonsParent.SettingCombineButtons(Multi_Managers.Data.UnitWindowDataByUnitFlags[flag].CombineUnitFlags);
        worldChangedButton.Setup(flag);
        unitSellButton.SetInfo(flag);
    }
}
