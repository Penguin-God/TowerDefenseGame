using System.Collections.Generic;
using System.Linq;

public class SkillDataGetter
{
    readonly IEnumerable<SkillUpgradeData> SkillUpgradeDatas;
    readonly SkillInventroy _skillInventroy;
    readonly Dictionary<SkillType, SkillLevelData> SkillLevelDatas;
    public SkillDataGetter(IEnumerable<SkillUpgradeData> skillUpgradeDatas, SkillInventroy skillInventroy, IEnumerable<SkillLevelData> skillLevelDatas)
    {
        SkillUpgradeDatas = skillUpgradeDatas;
        _skillInventroy = skillInventroy;
        SkillLevelDatas = skillLevelDatas.ToDictionary(x => x.SkillType, x => x);
    }

    public int GetSkillLevel(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).Level;
    public int GetSkillExp(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).HasAmount;

    public SkillUpgradeData GetSkillUpgradeData(SkillType skillType) => GetSkillUpgradeData(GetSkillLevel(skillType));
    public SkillUpgradeData GetSkillUpgradeData(int level)
    {
        if (level > SkillUpgradeDatas.Count()) return new SkillUpgradeData();
        return SkillUpgradeDatas.First(x => x.Level == level);
    }
    public int GetNeedLevelUpExp(SkillType skillType) => GetSkillUpgradeData(_skillInventroy.GetSkillInfo(skillType).Level).NeedExp;
    public bool SkillIsMax(SkillType skillType) => GetSkillLevel(skillType) >= SkillLevelDatas[skillType].MaxLevel;

    // ���� �������� �������� �ʿ��� ��ų ���� ���� - ������ ��ų ����
    public int CalculateHasableExpAmount(SkillType skillType) => SkillUpgradeDatas.Skip(GetSkillLevel(skillType) - 1).Sum(x => x.NeedExp) - GetSkillExp(skillType);
}
