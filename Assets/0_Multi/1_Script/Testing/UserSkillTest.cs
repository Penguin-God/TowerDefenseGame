using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillTest : MonoBehaviour
{
    Dictionary<SkillType, bool> _skillTypeByFlag = new Dictionary<SkillType, bool>();
    
    void Awake()
    {
        foreach (SkillType item in System.Enum.GetValues(typeof(SkillType)))
            _skillTypeByFlag.Add(item, false);
    }

    public void ActiveSkill(SkillType skillType)
    {
        _skillTypeByFlag[skillType] = true;
        new UserSkillFactory().GetSkill(skillType, 1).InitSkill();
    }
}
