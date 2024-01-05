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

public class SkillInventroy
{
    readonly Dictionary<SkillType, PlayerOwnedSkillInfo> _skillDatas;
    public SkillInventroy(Dictionary<SkillType, PlayerOwnedSkillInfo> skillDatas) => _skillDatas = skillDatas;

    public bool HasSkill(SkillType type) => _skillDatas.ContainsKey(type);

    public void AddSkill(SkillType skillType) => AddSkill(new SkillAmountData(skillType, 1));
    public void AddSkill(SkillAmountData data)
    {
        if (HasSkill(data.SkillType))
            _skillDatas[data.SkillType] = _skillDatas[data.SkillType].AddAmount(data.Amount);
        else
            _skillDatas.Add(data.SkillType, new PlayerOwnedSkillInfo(level: 1, data.Amount));
    }
    public void AddSkills(IEnumerable<SkillAmountData> datas)
    {
        foreach (var data in datas)
            AddSkill(data);
    }

    public void LevelUpSkill(SkillType skillType, int useAmount)
    {
        var skillInfo = GetSkillInfo(skillType);
        if (0 >= skillInfo.Level || useAmount > GetSkillInfo(skillType).HasAmount) return;
        _skillDatas[skillType] = new PlayerOwnedSkillInfo(skillInfo.Level + 1, skillInfo.HasAmount - useAmount);
    }

    public PlayerOwnedSkillInfo GetSkillInfo(SkillType type)
    {
        if (_skillDatas.TryGetValue(type, out var result) == false)
            return new PlayerOwnedSkillInfo();
        return result;
    }
    public IEnumerable<SkillType> GetAllHasSkills() => _skillDatas.Keys;
}
