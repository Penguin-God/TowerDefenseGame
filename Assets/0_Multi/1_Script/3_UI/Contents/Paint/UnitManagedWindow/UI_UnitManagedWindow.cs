using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Popup
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
        //_windowData = ;
        _description.text = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].Description;

        _combineButtonsParent.SettingCombineButtons(Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].CombineDatas);
    }
}
