using System.Collections;
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

    public SkillUpgradeData GetSkillUpgradeData(SkillType skillType) => GetSkillUpgradeData(GetSkillLevel(skillType));
    public SkillUpgradeData GetSkillUpgradeData(int level) => SkillUpgradeDatas.First(x => x.Level == level);

    public int GetSkillLevel(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).Level;
    public int GetSkillExp(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).HasAmount;
    public int GetNeedLevelUpExp(SkillType skillType) => GetSkillUpgradeData(_skillInventroy.GetSkillInfo(skillType).Level).NeedExp;
}
