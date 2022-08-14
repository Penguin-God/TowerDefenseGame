using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountTracker : Multi_UI_Scene
{
    Text unitCountText;
    protected override void Init()
    {
        base.Init();
        unitCountText = GetComponentInChildren<Text>();
        //Multi_UnitManager.Instance.OnCurrentUnitChanged += UpdateUnitCountText;
        Multi_UnitManager.Count.OnUnitCountChanged += UpdateUnitCountText;
    }

    void UpdateUnitCountText(int count)
    {
        unitCountText.text = $"최대 유닛 갯수 {count}/{Multi_GameManager.instance.MaxUnitCount}";
    }
}
