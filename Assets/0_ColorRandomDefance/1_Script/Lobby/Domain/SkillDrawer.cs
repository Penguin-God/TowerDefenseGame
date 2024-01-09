using System;
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
    IEnumerable<UserSkill> _drawableSkills;
    public SkillDrawer(IEnumerable<UserSkill> userSkillDatas) => _drawableSkills = userSkillDatas;

    public IEnumerable<SkillAmountData> DrawSkills(IEnumerable<SkillDrawInfo> drawInfos)
    {
        List<SkillAmountData> result = new();
        foreach (var info in drawInfos)
        {
            // ������ Ŭ�����̸鼭 �̹� ���� ��ų�� ����
            IReadOnlyList<SkillType> drawableSkills
                = _drawableSkills
                .Where(x => info.SkillClass == x.SkillClass)
                .Select(x => x.SkillType)
                .Except(result.Select(x => x.SkillType))
                .ToList();

            int drawAmount = Random.Range(info.MinCount, info.MaxCount + 1);
            if (drawableSkills.Count == 0)
                result.Add(new SkillAmountData(SkillType.None, drawAmount));
            else
                result.Add(new SkillAmountData(drawableSkills[Random.Range(0, drawableSkills.Count)], drawAmount));
        }
        return result;
    }
}
