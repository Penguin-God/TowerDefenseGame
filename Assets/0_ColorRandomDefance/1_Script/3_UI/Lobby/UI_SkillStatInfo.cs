using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillStatInfo : UI_Base
{
    [SerializeField] TextMeshProUGUI _statText;
    
    public void ShowSkillStat(SkillType skillType, int frameIndex)
    {
        string text = Managers.Data.UserSkill.GetSkillGoodsData(skillType).StatInfoFraems[frameIndex].Replace("{data}", GetSkillStat(skillType, frameIndex));
        _statText.text = DatabaseUtility.RelpaceKeyToValue(text);
    }

    string GetSkillStat(SkillType skillType, int frameIndex)
    {
        float result = Managers.ClientData.GetSkillLevelData(skillType).BattleDatas[frameIndex];
        switch (skillType)
        {
            case SkillType.보스데미지증가: result += 1; break;
            case SkillType.태극스킬: result += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(0, frameIndex)].Damage; break;
            case SkillType.검은유닛강화: result += Managers.Data.Unit.UnitStatByFlag[new UnitFlags(7, frameIndex)].Damage; break;
        }
        if (result >= 1000) return result.ToString("#,##0");
        else return result.ToString();
    }
}
