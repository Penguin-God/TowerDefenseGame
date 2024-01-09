using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Random = UnityEngine.Random;

public readonly struct SkillDrawInfo
{
    public readonly UserSkillClass SkillClass;
    public readonly int MinCount;
    public readonly int MaxCount;

    public SkillDrawInfo(UserSkillClass skillClass, int minCount, int maxCount)
    {
        if(maxCount <= 0)
            throw new ArgumentException($"최소가 0보다 작거나 같음. minCount : {minCount}");
        if (minCount > maxCount)
            throw new ArgumentException($"최소 개수가 최대 개수보다 큼. minCount : {minCount}, maxCount : {maxCount}");
        SkillClass = skillClass;
        MinCount = minCount;
        MaxCount = maxCount;
    }
}

public readonly struct SkillAmountData
{
    public readonly SkillType SkillType;
    public readonly int Amount;

    public SkillAmountData(SkillType skillType, int amount)
    {
        SkillType = skillType;
        Amount = amount;
    }
}

public readonly struct SkillDrawResult
{
    public readonly UserSkill Skill;
    public readonly int Amount;

    public SkillDrawResult(UserSkill skill, int amount)
    {
        Skill = skill;
        Amount = amount;
    }
}

public readonly struct UserSkill
{
    public readonly SkillType SkillType;
    public readonly UserSkillClass SkillClass;

    public UserSkill(SkillType skillType, UserSkillClass skillClass)
    {
        SkillType = skillType;
        SkillClass = skillClass;
    }
}

public class SkillDrawer
{
    IEnumerable<UserSkill> _drawableSkills;
    public SkillDrawer(IEnumerable<UserSkill> userSkillDatas) => _drawableSkills = userSkillDatas;

    public IEnumerable<SkillDrawResult> DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        List<SkillDrawResult> result = new();
        foreach (var info in drawInfos)
        {
            // 동일한 클래스이면서 이미 뽑은 스킬은 제외
            IReadOnlyList<SkillType> drawableSkills
                = _drawableSkills
                .Where(x => info.SkillClass == x.SkillClass)
                .Select(x => x.SkillType)
                .Except(result.Select(x => x.Skill.SkillType))
                .ToList();

            int drawAmount = Random.Range(info.MinCount, info.MaxCount + 1);
            if (drawableSkills.Count == 0)
                result.Add(new SkillDrawResult(new UserSkill(SkillType.None, UserSkillClass.Main), drawAmount));
            else
                result.Add(new SkillDrawResult(new UserSkill(drawableSkills[Random.Range(0, drawableSkills.Count)], info.SkillClass), drawAmount));
        }
        return result;
    }
}
