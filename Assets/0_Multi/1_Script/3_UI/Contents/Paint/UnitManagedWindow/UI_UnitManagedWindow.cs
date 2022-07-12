using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Popup
{
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
        _description.text = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].Description;
        _combineButtonsParent.SettingCombineButtons(Multi_Managers.Data.UnitWindowDataByUnitFlags[flags].CombineDatas);
    }
}
