using System.Collections.Generic;
using System.Linq;

public class SkillDataGetter
{
    readonly IEnumerable<SkillUpgradeData> SkillUpgradeDatas;
    readonly SkillInventroy _skillInventroy;

    public SkillDataGetter(IEnumerable<SkillUpgradeData> skillUpgradeDatas, SkillInventroy skillInventroy)
    {
        SkillUpgradeDatas = skillUpgradeDatas;
        _skillInventroy = skillInventroy;
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
    public bool SkillIsMax(SkillType skillType) => GetSkillLevel(skillType) >= SkillUpgradeDatas.Count() + 1;
}
