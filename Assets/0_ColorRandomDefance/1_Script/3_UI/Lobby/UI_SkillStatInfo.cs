using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillStatInfo : UI_Base
{
    [SerializeField] TextMeshProUGUI _statText;
    
    public void ShowSkillStat(SkillType skillType, int frameIndex)
    {
        int skillStat = (int)Managers.ClientData.GetSkillLevelData(skillType).BattleDatas[frameIndex];
        string text = Managers.Data.UserSkill.GetSkillGoodsData(skillType).StatInfoFraems[frameIndex].Replace("{data}", skillStat.ToString("#,##0"));
        _statText.text = DatabaseUtility.RelpaceKeyToValue(text);
    }
}
