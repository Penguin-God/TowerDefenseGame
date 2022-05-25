using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Scene
{
    protected override void Init()
    {
        base.Init();
        _combineButton.onClick.AddListener(Combine);
    }

    [SerializeField] UI_UnitWindowData _windowData;
    [SerializeField] Text _description;
    [SerializeField] Text _combineUnitName;
    [SerializeField] Button _combineButton;

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
        _combineUnitName.text = _windowData.CombineUnitName;
    }

    void Combine()
    {
        if (Multi_UnitManager.Instance.CheckCombineable(_windowData.CombineData.Conditions))
            Multi_SpawnManagers.NormalUnit.Spawn(_windowData.CombineUnitFlags);
    }
}
