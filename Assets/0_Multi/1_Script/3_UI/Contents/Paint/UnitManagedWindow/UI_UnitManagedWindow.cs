using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Base
{
    [SerializeField] UI_UnitWindowData _windowData;
    [SerializeField] Text _description;
    [SerializeField] UI_CombineButtonParent _combineButtonsParent;

    [SerializeField] Text _currentWolrd;

    public void Show(UnitFlags flags)
    {
        SetInfo(flags);
        gameObject.SetActive(true);
    }

    void SetInfo(UnitFlags flags)
    {
        _windowData = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags];
        _description.text = _windowData.Description;

        _combineButtonsParent.SettingCombineButtons(_windowData.CombineDatas);
    }
}
