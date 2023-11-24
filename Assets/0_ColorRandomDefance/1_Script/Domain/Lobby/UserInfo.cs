using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct PlayerOwnedSkillInfo
{
    readonly public int Level;
    readonly public int HasAmount;

    public PlayerOwnedSkillInfo(int level, int hasAmount)
    {
        Level = level;
        HasAmount = hasAmount;
    }

    public PlayerOwnedSkillInfo AddAmount(int amount) => new PlayerOwnedSkillInfo(Level, HasAmount + amount);
}

public struct UserInfo
{
    public string Name;
    readonly Dictionary<SkillType, PlayerOwnedSkillInfo> _skillDatas;
    
    public UserInfo(string name, Dictionary<SkillType, PlayerOwnedSkillInfo> skillDatas)
    {
        Name = name;
        _skillDatas = skillDatas;
    }

    public bool HasSkill(SkillType type) => _skillDatas.ContainsKey(type);
    public void AddSkill(SkillType type, int amount)
    {
        if (HasSkill(type))
            _skillDatas[type] = _skillDatas[type].AddAmount(amount);
        else
            _skillDatas.Add(type, new PlayerOwnedSkillInfo(level: 1, amount));
    }
    public PlayerOwnedSkillInfo GetSkillInfo(SkillType type) => _skillDatas[type];
}
