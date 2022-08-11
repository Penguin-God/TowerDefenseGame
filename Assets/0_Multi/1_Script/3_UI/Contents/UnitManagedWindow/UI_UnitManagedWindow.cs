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

    void SetInfo(UnitFlags flags)
    {
        _description.text = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].Description;
        _combineButtonsParent.SettingCombineButtons(Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].CombineDatas);
        worldChangedButton.Setup(flags);
        unitSellButton.SetInfo(flags);
    }
}
