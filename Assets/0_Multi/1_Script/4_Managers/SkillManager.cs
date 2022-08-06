using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill
{
    public string Name;
    public int Id;
    public const string path = "SkillData";

    public bool HasSkill;
    public bool EquipSkill;

    public void SetHasSkill(bool hasSkill)
    {
        HasSkill = hasSkill;
    }

    public void SetEquipSkill(bool equipSkill)
    {
        EquipSkill = equipSkill;
    }

    public virtual void InitSkill(SkillType skillType)
    {

    }
}

// TODO : 곧 죽음
public class SkillManager
{
    Dictionary<SkillType, System.Action> keyValuePairs = new Dictionary<SkillType, System.Action>();
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

    public void Clear()
    {
        keyValuePairs.Clear();
        skills.Clear();
    }
}
