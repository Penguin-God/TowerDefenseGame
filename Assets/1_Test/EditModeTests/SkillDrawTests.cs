using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawTests
{
    readonly IEnumerable<UserSkill> AllSkills = new List<UserSkill>() 
    {
        new UserSkill(SkillType.태극스킬, UserSkillClass.Main),
        new UserSkill(SkillType.흑의결속, UserSkillClass.Main),
        new UserSkill(SkillType.마나물약, UserSkillClass.Sub),
        new UserSkill(SkillType.거인학살자, UserSkillClass.Sub),
    };

    SkillInventroy CreateInventory(int 태극Lv = 1, int 흑결LV = 1, int 마나LV = 1, int 거학LV = 1) => new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
    {
        { SkillType.태극스킬, new PlayerOwnedSkillInfo(태극Lv, 0) }, 
        { SkillType.흑의결속, new PlayerOwnedSkillInfo(흑결LV, 0) },
        { SkillType.마나물약, new PlayerOwnedSkillInfo(마나LV, 0) },
        { SkillType.거인학살자, new PlayerOwnedSkillInfo(거학LV, 0) },
    });

    [Test]
    public void 더_이상_얻을_수_없는_스킬은_제외해야_함()
    {

    }

    [Test]
    [TestCase(1, 0, 0, 0)]
    [TestCase(1, 10, 10, 3000)]
    [TestCase(2, 0, 0, 6000)]
    [TestCase(2, 10, 0, 7800)]
    [TestCase(2, 10, 10, 9000)]
    public void 뽑기가_초과되면_보상으로_골드를_지급해야_함(int mainCount, int mainAmount, int subAmount, int expected)
    {
        var inventory = CreateInventory(태극Lv: 3, 흑결LV: 5, 마나LV:3);
        inventory.AddSkill(new SkillAmountData(SkillType.태극스킬, mainAmount));
        inventory.AddSkill(new SkillAmountData(SkillType.마나물약, subAmount));
        var skillUpgradeDatas = new SkillUpgradeData[]
        {
            CreateUpgradeData(1, 2),
            CreateUpgradeData(2, 4),
            CreateUpgradeData(3, 8),
            CreateUpgradeData(4, 16),
        };
        var drawInfos = new SkillDrawInfo[] { new SkillDrawInfo(UserSkillClass.Sub, 20, 20) };
        drawInfos.Concat(Enumerable.Repeat(new SkillDrawInfo(UserSkillClass.Main, 20, 20), mainCount));

        var sut = new SkillDrawController(AllSkills, new SkillDataGetter(skillUpgradeDatas, inventory, new SkillLevelData[] { }), new SkillRewardData(300, 200));

        var result = sut.DrawSkills(drawInfos);

        Assert.AreEqual(expected, result.RewardGoldWhenDrawOver);
    }

    SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() { Level = level, NeedExp = needExp };
}
