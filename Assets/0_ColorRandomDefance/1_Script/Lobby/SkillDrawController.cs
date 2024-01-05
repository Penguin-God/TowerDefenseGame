using System.Collections.Generic;
using System.Linq;

public readonly struct SkillDrawResultData
{
    public readonly IEnumerable<SkillAmountData> DrawSkills;
    public readonly int RewardGoldWhenDrawOver;

    public SkillDrawResultData(IEnumerable<SkillAmountData> drawSkills, int gold)
    {
        DrawSkills = drawSkills;
        RewardGoldWhenDrawOver = gold;
    }
}

public readonly struct SkillRewardData
{
    public readonly int MainSkillReward;
    public readonly int SubSkillReward;

    public SkillRewardData(int mainSkillReward, int subSkillReward)
    {
        MainSkillReward = mainSkillReward;
        SubSkillReward = subSkillReward;
    }
}

public class SkillDrawController
{
    readonly IEnumerable<UserSkill> AllUserSkills;
    readonly SkillDataGetter _skillDataGetter;
    readonly SkillDrawer _skillDrawer = new SkillDrawer(null);
    readonly SkillRewardData SkillRewardData;
    public SkillDrawController(IEnumerable<UserSkill> allUserSkills, SkillDataGetter skillDataGetter, SkillRewardData skillRewardData)
    {
        AllUserSkills = allUserSkills;
        _skillDataGetter = skillDataGetter;
        SkillRewardData = skillRewardData;
    }

    public SkillDrawResultData DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        return new SkillDrawResultData(_skillDrawer.DrawSkills(drawInfos), 0);
    }

    IEnumerable<UserSkill> GetDrawableSkills() => AllUserSkills.Where(x => _skillDataGetter.GetSkillExp(x.SkillType) > 0);
}
