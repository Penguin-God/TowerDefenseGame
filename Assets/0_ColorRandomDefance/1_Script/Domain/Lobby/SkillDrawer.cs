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
            throw new ArgumentException($"�ּҰ� 0���� �۰ų� ����. minCount : {minCount}");
        if (minCount > maxCount)
            throw new ArgumentException($"�ּ� ������ �ִ� �������� ŭ. minCount : {minCount}, maxCount : {maxCount}");
        SkillClass = skillClass;
        MinCount = minCount;
        MaxCount = maxCount;
    }
}

public readonly struct SkillDrawResultInfo
{
    public readonly SkillType SkillType;
    public readonly int Amount;

    public SkillDrawResultInfo(SkillType skillType, int amount)
    {
        SkillType = skillType;
        Amount = amount;
    }
}

public class SkillDrawer
{
    readonly IReadOnlyDictionary<UserSkillClass, IReadOnlyList<SkillType>> _skillByClass;
    public SkillDrawer(IReadOnlyDictionary<UserSkillClass, IReadOnlyList<SkillType>> skillByClass) => _skillByClass = skillByClass;

    public IEnumerable<SkillDrawResultInfo> DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        List<SkillDrawResultInfo> result = new();
        foreach (var info in drawInfos)
        {
            IReadOnlyList<SkillType> drawableSkills = _skillByClass[info.SkillClass].Except(result.Select(x => x.SkillType)).ToList();
            SkillType drawSkill = drawableSkills[Random.Range(0, drawableSkills.Count)];
            int drawAmount = Random.Range(info.MinCount, info.MaxCount + 1);
            result.Add(new SkillDrawResultInfo(drawSkill, drawAmount));
        }
        return result;
    }
}
