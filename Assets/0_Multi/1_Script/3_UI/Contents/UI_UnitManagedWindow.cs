using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Scene
{
    protected override void Init()
    {
        base.Init();
    }

    [SerializeField] Text _description;
    [SerializeField] Text _currentWolrd;
    [SerializeField] Text _combineUnitName;

    public void Show(UnitFlags flags)
    {
        gameObject.SetActive(true);

        UI_UnitWindowData data = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags];
        _description.text = data.Description;
        _combineUnitName.text = data.CombineUnitName;
    }
}
