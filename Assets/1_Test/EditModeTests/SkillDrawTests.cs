using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawTests
{
    readonly IEnumerable<UserSkill> AllSkills = new List<UserSkill>() 
    {
        new UserSkill(SkillType.�±ؽ�ų, UserSkillClass.Main),
        new UserSkill(SkillType.���ǰ��, UserSkillClass.Main),
        new UserSkill(SkillType.��������, UserSkillClass.Main),
        new UserSkill(SkillType.�����л���, UserSkillClass.Main),
    };

    SkillInventroy CreateInventory(int lv1 = 1, int lv2 = 1, int lv3 = 1, int lv4 = 1) => new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
    {
        { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(lv1, 0) }, 
        { SkillType.���ǰ��, new PlayerOwnedSkillInfo(lv2, 0) },
        { SkillType.��������, new PlayerOwnedSkillInfo(lv3, 0) },
        { SkillType.�����л���, new PlayerOwnedSkillInfo(lv4, 0) },
    });

    [Test]
    public void ��_�̻�_����_��_����_��ų��_�����ؾ�_��()
    {

    }

    [Test]
    public void �̱Ⱑ_�ʰ��Ǹ�_��������_��带_�����ؾ�_��()
    {
        var inventory = CreateInventory(5, 5);
        var skillUpgradeDatas = new SkillUpgradeData[]
        {
            CreateUpgradeData(1, 2),
            CreateUpgradeData(2, 4),
            CreateUpgradeData(3, 8),
            CreateUpgradeData(4, 16),
        };
        var sut = new SkillDrawController(AllSkills, new SkillDataGetter(skillUpgradeDatas, inventory, null), new SkillRewardData(300, 200));

        var result = sut.DrawSkills(new SkillDrawInfo[] { new SkillDrawInfo(UserSkillClass.Main, 3, 3)});

        Assert.AreEqual(900, result.RewardGoldWhenDrawOver);
    }

    SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() { Level = level, NeedExp = needExp };
}
