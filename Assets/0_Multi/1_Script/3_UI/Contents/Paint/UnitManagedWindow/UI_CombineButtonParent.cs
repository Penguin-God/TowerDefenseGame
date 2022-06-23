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
        _combineButtons.ToList().ForEach(x => x.gameObject.SetActive(false));
        _combineButtons.ToList().ForEach(x => x.onClick.RemoveAllListeners());

        for (int i = 0; i < datas.Count; i++)
        {
            _combineButtons[i].gameObject.SetActive(true);

            CombineData data = new CombineData(datas[i].UnitFlags, datas[i].KoearName);
            // 진짜 이유는 1도 모르겠지만 datas[i] 형식으로 넣어서 구독하면 에러 뜸
            _combineButtons[i].onClick.AddListener(() => Combine(data));
            _combineButtons[i].GetComponentInChildren<Text>(true).text = datas[i].KoearName;
        }
    }
    void Combine(CombineData data) => Multi_UnitManager.Instance.Combine(data);
    //{
    //    print($"컴바인 시도 : 색깔 : {data.UnitFlags.ColorNumber}, 클래스 : {data.UnitFlags.ClassNumber}");
    //    if (Multi_UnitManager.Instance.CheckCombineable(data.Conditions))
    //    {
    //        Multi_UnitManager.Instance.SacrificedUnit_ForCombine(data.Conditions);
    //        Multi_SpawnManagers.NormalUnit.Spawn(data.UnitFlags);
    //    }
    //}
}
