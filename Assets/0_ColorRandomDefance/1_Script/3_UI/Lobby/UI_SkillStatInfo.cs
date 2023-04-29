using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillStatInfo : UI_Base
{
    [SerializeField] TextMeshProUGUI _statText;
    
    public void ShowSkillStat(SkillType skillType, int frameIndex)
    {
        int stat = (int)Managers.ClientData.GetSkillLevelData(skillType).BattleDatas[frameIndex];
        _statText.text = Managers.Data.UserSkill.GetSkillGoodsData(skillType).StatInfoFraems[frameIndex].Replace("{data}", stat.ToString("#,##0"));
    }
}
