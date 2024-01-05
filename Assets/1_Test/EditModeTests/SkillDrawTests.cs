using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawTests
{
    readonly IEnumerable<UserSkill> AllSkills = new List<UserSkill>() 
    {
        new UserSkill(SkillType.태극스킬, UserSkillClass.Main),
        new UserSkill(SkillType.흑의결속, UserSkillClass.Main),
        new UserSkill(SkillType.마나물약, UserSkillClass.Main),
        new UserSkill(SkillType.거인학살자, UserSkillClass.Main),
    };

    SkillInventroy CreateInventory(int lv1 = 1, int lv2 = 1, int lv3 = 1, int lv4 = 1) => new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
    {
        { SkillType.태극스킬, new PlayerOwnedSkillInfo(lv1, 0) }, 
        { SkillType.흑의결속, new PlayerOwnedSkillInfo(lv2, 0) },
        { SkillType.마나물약, new PlayerOwnedSkillInfo(lv3, 0) },
        { SkillType.거인학살자, new PlayerOwnedSkillInfo(lv4, 0) },
    });

    [Test]
    public void 더_이상_얻을_수_없는_스킬은_제외해야_함()
    {

    }

    [Test]
    public void 뽑기가_초과되면_보상으로_골드를_지급해야_함()
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
