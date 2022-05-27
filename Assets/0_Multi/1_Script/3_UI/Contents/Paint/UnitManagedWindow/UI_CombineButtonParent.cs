using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_CombineButtonParent : Multi_UI_Base
{
    [SerializeField] Button[] _combineButtons;

    public void SettingCombineButtons(IReadOnlyList<CombineData> datas)
    {
        InActiveAllButton();
        for (int i = 0; i < datas.Count; i++)
        {
            _combineButtons[i].gameObject.SetActive(true);
            _combineButtons[i].onClick.AddListener(() => Combine(datas[i]));
            _combineButtons[i].GetComponentInChildren<Text>(true).text = datas[i].KoearName;
        }
    }

    void InActiveAllButton() => _combineButtons.ToList().ForEach(x => x.gameObject.SetActive(false));

    void Combine(CombineData data)
    {
        print($"컴바인 시도 : 색깔 : {data.UnitFlags.ColorNumber}, 클래스 : {data.UnitFlags.ClassNumber}");
        if (Multi_UnitManager.Instance.CheckCombineable(data.Conditions))
        {
            Multi_UnitManager.Instance.SacrificedUnit_ForCombine(data.Conditions);
            Multi_SpawnManagers.NormalUnit.Spawn(data.UnitFlags);
        }
    }
}
