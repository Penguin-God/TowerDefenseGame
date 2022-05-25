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

    [SerializeField] UnitFlags _unitFlags;
    [SerializeField] UnitFlags _combineUnitFlags;
    [SerializeField] Text _description;
    [SerializeField] Text _currentWolrd;
    [SerializeField] Text _combineUnitName;
    [SerializeField] Button _combineButton;

    public void Show(UnitFlags flags)
    {
        SetInfo(flags);
        gameObject.SetActive(true);
    }

    void SetInfo(UnitFlags flags)
    {
        UI_UnitWindowData data = Multi_Managers.Data.UnitWindowDataByUnitFlags[flags];
        _unitFlags = data.UnitFlags;
        _combineUnitFlags = data.CombineUnitFlags;
        _description.text = data.Description;
        _combineUnitName.text = data.CombineUnitName;
    }

    void Combine()
    {
        if (Multi_UnitManager.Instance.CheckCombineable(Multi_Managers.Data.CombineDataByUnitFlags[_combineUnitFlags].Conditions))
        {
            Multi_SpawnManagers.NormalUnit.Spawn(_combineUnitFlags);
            //print($"소환된 유닛 컬러 {_unitFlags.ColorNumber}, 클래스 {_unitFlags.ClassNumber}");
        }
        else print("숫자 부족!!!!!!!!!!!!!!!");
        // Multi_SpawnManagers.NormalUnit.Spawn(_unitFlags);
    }
}
