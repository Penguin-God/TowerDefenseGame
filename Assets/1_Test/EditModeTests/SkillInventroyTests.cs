using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillInventroyTests
{
    readonly IEnumerable<SkillLevelData> SkillLevelDatas = new SkillLevelData[]
    {
        CreateLevelData(SkillType.�±ؽ�ų, 5),
        CreateLevelData(SkillType.�÷�������, 1),
    };

    static SkillLevelData CreateLevelData(SkillType skillType, int maxLevel) => new SkillLevelData() { SkillType = skillType, MinLevel = 1, MaxLevel = maxLevel };

    [Test]
    public void ��ų��_������_�κ��丮��_����Ǿ��_��()
    {
        // Arrange
        var sut = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>());

        const int GetAmount = 7;
        var drawInfos = new List<SkillAmountData>
        {
            new SkillAmountData(SkillType.�±ؽ�ų, GetAmount),
            new SkillAmountData(SkillType.���ǰ��, GetAmount),
            new SkillAmountData(SkillType.�����л���, GetAmount),
        };

        // Act
        sut.AddSkills(drawInfos);

        // Assert
        AssertSkill(SkillType.�±ؽ�ų);
        AssertSkill(SkillType.���ǰ��);
        AssertSkill(SkillType.�����л���);

        void AssertSkill(SkillType skillType)
        {
            Assert.IsTrue(sut.HasSkill(skillType));
            var skillData = sut.GetSkillInfo(skillType);
            Assert.AreEqual(skillData.HasAmount, GetAmount);
        }
    }

    [Test]
    [TestCase(5, true)]
    [TestCase(3, false)]
    public void ��ų_����_�����͸�_��ȯ�ؾ�_��(int level, bool expected)
    {
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
        { 
            { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(level, 0) },
            { SkillType.�÷�������, new PlayerOwnedSkillInfo(level, 1) },
        });
        var skillUpgradeDatas = Enumerable.Repeat(new SkillUpgradeData(), 4);
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory, SkillLevelDatas);

        Assert.AreEqual(expected, sut.SkillIsMax(SkillType.�±ؽ�ų));
        Assert.IsTrue(sut.SkillIsMax(SkillType.�÷�������));
    }

    [Test]
    [TestCase(1, 0, 30)]
    [TestCase(1, 10, 20)]
    [TestCase(1, 30, 0)]
    [TestCase(3, 0, 24)]
    [TestCase(4, 5, 11)]
    [TestCase(5, 0, 0)]
    public void ����_����_��_�ִ�_��ų��_������_����ؾ�_��(int level, int hasAmount, int expected)
    {
        var skillUpgradeDatas = new SkillUpgradeData[]
        {
            CreateUpgradeData(1, 2),
            CreateUpgradeData(2, 4),
            CreateUpgradeData(3, 8),
            CreateUpgradeData(4, 16),
        };
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
        { 
            { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(level, hasAmount) },
            { SkillType.�÷�������, new PlayerOwnedSkillInfo(1, 1) },
        });
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory, SkillLevelDatas);

        var result = sut.CalculateHasableExpAmount(SkillType.�±ؽ�ų);

        Assert.AreEqual(expected, result);
        Assert.Zero(sut.CalculateHasableExpAmount(SkillType.�÷�������));
    }

    SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() {Level = level, NeedExp = needExp};
}
