using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawTests
{
    readonly IEnumerable<UserSkill> AllSkills = new List<UserSkill>() 
    {
        new UserSkill(SkillType.�±ؽ�ų, UserSkillClass.Main),
        new UserSkill(SkillType.���ǰ��, UserSkillClass.Main),
        new UserSkill(SkillType.��������, UserSkillClass.Sub),
        new UserSkill(SkillType.�����л���, UserSkillClass.Sub),
    };

    SkillInventroy CreateInventory(int �±�Lv = 1, int ���LV = 1, int ����LV = 1, int ����LV = 1) => new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
    {
        { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(�±�Lv, 0) }, 
        { SkillType.���ǰ��, new PlayerOwnedSkillInfo(���LV, 0) },
        { SkillType.��������, new PlayerOwnedSkillInfo(����LV, 0) },
        { SkillType.�����л���, new PlayerOwnedSkillInfo(����LV, 0) },
    });

    [Test]
    public void ��_�̻�_����_��_����_��ų��_�����ؾ�_��()
    {

    }

    [Test]
    [TestCase(1, 0, 0, 0)]
    [TestCase(1, 10, 10, 3000)]
    [TestCase(2, 0, 0, 6000)]
    [TestCase(2, 10, 0, 7800)]
    [TestCase(2, 10, 10, 9000)]
    public void �̱Ⱑ_�ʰ��Ǹ�_��������_��带_�����ؾ�_��(int mainCount, int mainAmount, int subAmount, int expected)
    {
        var inventory = CreateInventory(�±�Lv: 3, ���LV: 5, ����LV:3);
        inventory.AddSkill(new SkillAmountData(SkillType.�±ؽ�ų, mainAmount));
        inventory.AddSkill(new SkillAmountData(SkillType.��������, subAmount));
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
