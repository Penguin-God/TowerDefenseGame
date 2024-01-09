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

    readonly IEnumerable<SkillUpgradeData> SkillUpgradeDatas = new SkillUpgradeData[]
    {
        CreateUpgradeData(1, 2),
        CreateUpgradeData(2, 4),
        CreateUpgradeData(3, 8),
        CreateUpgradeData(4, 16),
    };

    readonly IEnumerable<SkillLevelData> SkillLevelDatas = new SkillLevelData[]
    {
        CreateLevelData(SkillType.�±ؽ�ų, 5),
        CreateLevelData(SkillType.���ǰ��, 5),
        CreateLevelData(SkillType.��������, 1),
        CreateLevelData(SkillType.�����л���, 5),
    };

    static SkillLevelData CreateLevelData(SkillType skillType, int maxLevel) => new SkillLevelData() { SkillType = skillType, MinLevel = 1, MaxLevel = maxLevel };

    SkillInventroy CreateInventory(int �±�Lv = 1, int ���LV = 1, int ����LV = 1) => new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
    {
        { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(�±�Lv, 0) }, 
        { SkillType.���ǰ��, new PlayerOwnedSkillInfo(���LV, 0) },
        { SkillType.��������, new PlayerOwnedSkillInfo(1, 0) },
        { SkillType.�����л���, new PlayerOwnedSkillInfo(����LV, 0) },
    });

    readonly SkillRewardData SkillRewardData = new SkillRewardData(300, 200);

    [Test]
    [TestCase(1, 1, 1, 3)]
    [TestCase(5, 1, 1, 2)]
    [TestCase(1, 5, 5, 1)]
    [TestCase(5, 5, 5, 0)]
    public void ��_�̻�_����_��_����_��ų��_�����ؾ�_��(int �±�Lv, int ���LV, int ����LV, int expected)
    {
        var inventory = CreateInventory(�±�Lv, ���LV, ����LV);
        var sut = CreateSut(inventory);
        IEnumerable<SkillDrawInfo> drawInfos = new SkillDrawInfo[] 
        { 
            new SkillDrawInfo(UserSkillClass.Main, 20, 20),
            new SkillDrawInfo(UserSkillClass.Main, 20, 20),
            new SkillDrawInfo(UserSkillClass.Sub, 20, 20),
            new SkillDrawInfo(UserSkillClass.Sub, 20, 20),
        };

        var reuslt = sut.DrawSkills(drawInfos);

        Assert.AreEqual(expected, reuslt.DrawSkills.Count());
    }

    [Test]
    [TestCase(1, 0, 0, 0)]
    [TestCase(1, 10, 10, 3000)]
    [TestCase(2, 0, 0, 6000)]
    [TestCase(2, 10, 0, 7800)]
    [TestCase(2, 10, 10, 9000)]
    public void �̱Ⱑ_�ʰ��Ǹ�_��������_��带_�����ؾ�_��(int mainCount, int mainAmount, int subAmount, int expected)
    {
        var inventroy = CreateInventory(�±�Lv: 3, ���LV: 5, ����LV:3);
        inventroy.AddSkill(new SkillAmountData(SkillType.�±ؽ�ų, mainAmount));
        inventroy.AddSkill(new SkillAmountData(SkillType.�����л���, subAmount));

        IEnumerable<SkillDrawInfo> drawInfos = new SkillDrawInfo[] { new SkillDrawInfo(UserSkillClass.Sub, 20, 20) };
        drawInfos = drawInfos.Concat(Enumerable.Repeat(new SkillDrawInfo(UserSkillClass.Main, 20, 20), mainCount));

        var sut = CreateSut(inventroy);

        var result = sut.DrawSkills(drawInfos);

        Assert.AreEqual(expected, result.RewardGoldWhenDrawOver);
    }

    SkillDrawController CreateSut(SkillInventroy inventroy) => new SkillDrawController(AllSkills, new SkillDataGetter(SkillUpgradeDatas, inventroy, SkillLevelDatas), SkillRewardData);

    static SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() { Level = level, NeedExp = needExp };
}
