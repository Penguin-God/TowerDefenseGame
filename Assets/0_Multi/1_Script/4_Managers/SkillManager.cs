using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    Dictionary<SkillType, System.Action> keyValuePairs;
    List<SkillType> skills = new List<SkillType>();

    public void Init()
    {
        skills.Add(SkillType.시작골드증가);

        for (int i = 0; i < skills.Count; i++)
        {
            if (Multi_Managers.ClientData.SkillByType[skills[i]].EquipSkill == true)
                Multi_Managers.ClientData.SkillByType[skills[i]].InitSkill(skills[i]);
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
        {
            Debug.Log("시작 골드 증가 사용");
        }
        else
        {
            Debug.Log("시작 골드 증가 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.시작식량증가].EquipSkill == true)
        {
            Debug.Log("시작 식량 증가 사용");
        }
        else
        {
            Debug.Log("시작 식량 증가 없음.....");
        }

        if (Multi_Managers.ClientData.SkillByType[SkillType.최대유닛증가].EquipSkill == true)
        {
            Debug.Log("시작 최대 유닛 증가 사용");
        }
        else
        {
            Debug.Log("시작 최대 유닛 증가 없음.....");
        }
    }
}
