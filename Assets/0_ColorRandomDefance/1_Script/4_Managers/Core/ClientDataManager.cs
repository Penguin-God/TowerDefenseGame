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

    public IEnumerable<SkillType> EquipSkills => _typeByClass.Values;

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

public class ClientDataManager
{
    EquipSkillManager _equipSkillManager = new EquipSkillManager(SkillType.None, SkillType.None);
    public EquipSkillManager EquipSkillManager => _equipSkillManager;

    Dictionary<SkillType, int> _skillByLevel = new Dictionary<SkillType, int>();
    public int GetSkillLevel(SkillType skillType)
    {
        if (_skillByLevel.TryGetValue(skillType, out int result))
            return result;
        else return 0;
    }
    // public UserSkillLevelData GetSkillLevelData(SkillType skillType) => Managers.Data.UserSkill.GetSkillLevelData(skillType, GetSkillLevel(skillType));

    Dictionary<SkillType, int> _skillByExp = new Dictionary<SkillType, int>();
    // public Dictionary<SkillType, int> SkillByExp => _skillByExp;
    //public void GetExp(SkillType skill, int getQuantity)
    //{
    //    if (_skillByLevel[skill] == 0)
    //        _skillByLevel[skill]++;

    //    _skillByExp[skill] += getQuantity;
    //}

    //public bool UpgradeSkill(SkillType skill)
    //{
    //    if (CanUpgrade(skill) == false) return false;

    //    _skillByExp[skill] -= GetSkillLevelData(skill).Exp;
    //    _skillByLevel[skill]++;
    //    return true;
    //}

    // bool CanUpgrade(SkillType skill) => _skillByExp[skill] >= GetSkillLevelData(skill).Exp;

    public void Init()
    {
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            if (type == SkillType.None) continue;
            _skillByLevel.Add(type, 0);
            _skillByExp.Add(type, 0);
        }
    }
}

public enum SkillType
{
    None,
    시작골드증가,
    마나물약,
    최대유닛증가,
    태극스킬,
    흑의결속,
    황금빛기사,
    컬러마스터,
    마나변이,
    마나불능,
    장사꾼,
    거인학살자,
    도박사,
    메테오,
    네크로맨서,
    마창사,
    썬콜,
    덫,
    백의결속,
    VIP,
    부익부,
    전설의기사,
}

public enum UserSkillClass
{
    Main,
    Sub,
}