using System.Collections;
using System.Collections.Generic;

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

public struct SkillInventroy
{
    readonly Dictionary<SkillType, PlayerOwnedSkillInfo> _skillDatas;
    public SkillInventroy(Dictionary<SkillType, PlayerOwnedSkillInfo> skillDatas) => _skillDatas = skillDatas;

    public bool HasSkill(SkillType type) => _skillDatas.ContainsKey(type);
    public void AddSkill(SkillType type, int amount)
    {
        if (HasSkill(type))
            _skillDatas[type] = _skillDatas[type].AddAmount(amount);
        else
            _skillDatas.Add(type, new PlayerOwnedSkillInfo(level: 1, amount));
    }

    public void AddSkill(SkillAmountData data)
    {
        if (HasSkill(data.SkillType))
            _skillDatas[data.SkillType] = _skillDatas[data.SkillType].AddAmount(data.Amount);
        else
            _skillDatas.Add(data.SkillType, new PlayerOwnedSkillInfo(level: 1, data.Amount));
    }
    public PlayerOwnedSkillInfo GetSkillInfo(SkillType type) => _skillDatas[type];
    public IEnumerable<SkillType> GetAllHasSkills() => _skillDatas.Keys;
}
