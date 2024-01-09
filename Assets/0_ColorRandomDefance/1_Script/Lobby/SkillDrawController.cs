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

    public int CalculateOverGold(UserSkillClass skillClass, int amount)
    {
        if(skillClass == UserSkillClass.Main) return MainSkillReward * amount;
        else return SubSkillReward * amount;
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
        List<SkillAmountData> drawSkills = new List<SkillAmountData>();
        int rewardGold = 0;
        foreach (var drawResult in new SkillDrawer(GetDrawableSkills()).DrawSkills(drawInfos))
        {
            if (drawResult.Skill.SkillType == SkillType.None) rewardGold += SkillRewardData.CalculateOverGold(drawResult.Skill.SkillClass, drawResult.Amount);
            else if(ChcekOverAmount(drawResult, out int overAmount))
            {
                rewardGold += SkillRewardData.CalculateOverGold(drawResult.Skill.SkillClass, overAmount);
                drawSkills.Add(new SkillAmountData(drawResult.Skill.SkillType, _skillDataGetter.CalculateHasableExpAmount(drawResult.Skill.SkillType)));
            }
            else
                drawSkills.Add(new SkillAmountData(drawResult.Skill.SkillType, drawResult.Amount));
        }
        return new SkillDrawResultData(drawSkills, rewardGold);
    }

    bool ChcekOverAmount(SkillDrawResult result, out int overAmount)
    {
        overAmount = result.Amount - _skillDataGetter.CalculateHasableExpAmount(result.Skill.SkillType);
        return overAmount > 0;
    }

    IEnumerable<UserSkill> GetDrawableSkills() => AllUserSkills.Where(x => _skillDataGetter.CalculateHasableExpAmount(x.SkillType) > 0);
}
