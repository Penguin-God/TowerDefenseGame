using System;
using System.Collections;
using System.Collections.Generic;
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
    IEnumerable<UserSkill> _userSkillDatas;
    public SkillDrawer(IEnumerable<UserSkill> userSkillDatas) => _userSkillDatas = userSkillDatas;

    public IEnumerable<SkillAmountData> DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        List<SkillAmountData> result = new();
        foreach (var info in drawInfos)
        {
            // 동일한 클래스이면서 이미 뽑은 스킬은 제외
            IReadOnlyList<SkillType> drawableSkills
                = _userSkillDatas
                .Where(x => info.SkillClass == x.SkillClass)
                .Select(x => x.SkillType)
                .Except(result.Select(x => x.SkillType))
                .ToList();
            SkillType drawSkill = drawableSkills[Random.Range(0, drawableSkills.Count)];
            int drawAmount = Random.Range(info.MinCount, info.MaxCount + 1);
            result.Add(new SkillAmountData(drawSkill, drawAmount));
        }
        return result;
    }
}
