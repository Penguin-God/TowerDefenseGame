using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotifySkillDrawWindow : UI_Popup
{
    [SerializeField] Transform _skillInfoParnet;

    public void ShowSkillsInfo(IEnumerable<SkillAmountData> drawSkillDatas)
    {
        foreach (Transform child in _skillInfoParnet)
            Destroy(child.gameObject);

        foreach (var data in drawSkillDatas)
            Managers.UI.MakeSubItem<UI_DrawSkillItem>(_skillInfoParnet).ShowDrawSkill(data);
    }
}
