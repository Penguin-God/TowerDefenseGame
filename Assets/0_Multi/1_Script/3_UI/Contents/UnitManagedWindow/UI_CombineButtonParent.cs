using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_CombineButtonParent : Multi_UI_Base
{
    [SerializeField] Button[] _combineButtons;

    public void SettingCombineButtons(IReadOnlyList<UnitFlags> flags)
    {
        _combineButtons.ToList().ForEach(x => x.gameObject.SetActive(false));
        _combineButtons.ToList().ForEach(x => x.onClick.RemoveAllListeners());

        for (int i = 0; i < flags.Count; i++)
        {
            _combineButtons[i].gameObject.SetActive(true);

            int newI = i;
            _combineButtons[i].onClick.AddListener(() => Combine(flags[newI]));
            _combineButtons[i].GetComponentInChildren<Text>(true).text = Multi_Managers.Data.UnitNameDataByFlag[flags[i]].KoearName;
        }
    }

    void Combine(UnitFlags flag) => Multi_UnitManager.Instance.TryCombine_RPC(flag);
}
