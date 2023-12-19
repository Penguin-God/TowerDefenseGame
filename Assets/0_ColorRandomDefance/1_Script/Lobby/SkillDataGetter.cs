using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillDataGetter
{
    readonly IEnumerable<SkillUpgradeData> SkillUpgradeDatas;
    readonly IEnumerable<UserSkillLevelData> UserSkillLevelDatas;
    readonly SkillInventroy _skillInventroy;

    public SkillDataGetter(IEnumerable<SkillUpgradeData> skillUpgradeDatas, IEnumerable<UserSkillLevelData> userSkillLevelDatas, SkillInventroy skillInventroy)
    {
        SkillUpgradeDatas = skillUpgradeDatas;
        UserSkillLevelDatas = userSkillLevelDatas;
        _skillInventroy = skillInventroy;
    }

    public SkillUpgradeData GetSkillUpgradeData(SkillType skillType) => GetSkillUpgradeData(GetSkillLevel(skillType));
    public SkillUpgradeData GetSkillUpgradeData(int level)
    {
        if (level > SkillUpgradeDatas.Count()) return new SkillUpgradeData();
        return SkillUpgradeDatas.First(x => x.Level == level);
    }

    public int GetSkillLevel(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).Level;
    public int GetSkillExp(SkillType skillType) => _skillInventroy.GetSkillInfo(skillType).HasAmount;
    public int GetNeedLevelUpExp(SkillType skillType) => GetSkillUpgradeData(_skillInventroy.GetSkillInfo(skillType).Level).NeedExp;
    public bool SkillIsMax(SkillType skillType) => GetSkillLevel(skillType) >= UserSkillLevelDatas.Where(x => x.SkillType == skillType).Count();
}
