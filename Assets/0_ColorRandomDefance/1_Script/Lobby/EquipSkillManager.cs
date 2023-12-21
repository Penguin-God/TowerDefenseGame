using System.Collections;
using System.Collections.Generic;
using System;

public class EquipSkillManager
{
    Dictionary<UserSkillClass, SkillType> _typeByClass = new Dictionary<UserSkillClass, SkillType>()
    {
        {UserSkillClass.Main, SkillType.None},
        {UserSkillClass.Sub, SkillType.None},
    };

    public EquipSkillManager(SkillType main, SkillType sub)
    {
        ChangedEquipSkill(UserSkillClass.Main, main);
        ChangedEquipSkill(UserSkillClass.Sub, sub);
    }

    public SkillType MainSkill => _typeByClass[UserSkillClass.Main];
    public SkillType SubSkill => _typeByClass[UserSkillClass.Sub];
    public bool AllSkillsEquipped => MainSkill != SkillType.None && SubSkill != SkillType.None;

    public event Action<UserSkillClass, SkillType> OnEquipSkillChanged = null;

    public void ChangedEquipSkill(UserSkillClass skillClass, SkillType skillType)
    {
        _typeByClass[skillClass] = skillType;
        OnEquipSkillChanged?.Invoke(skillClass, skillType);
    }

    public void AllUnEquip()
    {
        ChangedEquipSkill(UserSkillClass.Main, SkillType.None);
        ChangedEquipSkill(UserSkillClass.Sub, SkillType.None);
    }
}
