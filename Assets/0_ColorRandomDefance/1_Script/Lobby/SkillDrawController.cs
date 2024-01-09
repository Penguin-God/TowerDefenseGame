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
    readonly SkillRewardData SkillRewardData;
    public SkillDrawController(IEnumerable<UserSkill> allUserSkills, SkillDataGetter skillDataGetter, SkillRewardData skillRewardData)
    {
        AllUserSkills = allUserSkills;
        _skillDataGetter = skillDataGetter;
        SkillRewardData = skillRewardData;
    }

    public SkillDrawResultData DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        int rewardGold = 0;
        // 뽑기 전 만렙인 경우 Main, Sub 남은 숫자별로 골드 더함. 안에서 None뱉기
        // 뽑은 후 EXP별로 골드 더하기. 비교 후 큰만큼
        return new SkillDrawResultData(new SkillDrawer(GetDrawableSkills()).DrawSkills(drawInfos), rewardGold);
    }

    IEnumerable<UserSkill> GetDrawableSkills() => AllUserSkills.Where(x => _skillDataGetter.GetSkillExp(x.SkillType) > 0);
}
