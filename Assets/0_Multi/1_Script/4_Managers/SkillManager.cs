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

    public virtual void InitSkill(Skill skill) { }

    // public abstract void InitSkill(SkillType skillType);
}

// TODO : 곧 죽음
public class SkillManager
{
    Dictionary<SkillType, System.Action> keyValuePairs = new Dictionary<SkillType, System.Action>();
    List<Skill> skills = new List<Skill>();

    public void Init()
    {
        
        if (Multi_Managers.ClientData.SkillByType[SkillType.태극스킬].EquipSkill == true)
        {
            skills.Add(new Taegeuk());
            Debug.Log("태극 스킬 추가");
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

        if (Multi_Managers.ClientData.SkillByType[SkillType.시작골드증가].EquipSkill == true)
        {
            Debug.Log("시작 골드 증가 사용");
        }
        else
        {
            Debug.Log("시작 골드 증가 없음.....");
        }

        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].EquipSkill == true)
                skills[i].InitSkill(skills[i]);
        }
    }

    

    public void Clear()
    {
        keyValuePairs.Clear();
        skills.Clear();
    }
}

public class PassiveSkill : Skill
{
    public override void InitSkill(Skill skill)
    {

    }
}

public class ActiveSkill : Skill
{
    public override void InitSkill(Skill skill)
    {

    }
}

public class Taegeuk : PassiveSkill
{
    public override void InitSkill(Skill skill)
    {
        Debug.Log("태극 시너지 스킬 발동");
    }
}
