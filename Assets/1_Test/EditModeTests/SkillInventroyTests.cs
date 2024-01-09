using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillInventroyTests
{
    readonly IEnumerable<SkillLevelData> SkillLevelDatas = new SkillLevelData[]
    {
        CreateLevelData(SkillType.태극스킬, 5),
        CreateLevelData(SkillType.컬러마스터, 1),
    };

    static SkillLevelData CreateLevelData(SkillType skillType, int maxLevel) => new SkillLevelData() { SkillType = skillType, MinLevel = 1, MaxLevel = maxLevel };

    [Test]
    public void 스킬을_넣으면_인벤토리에_저장되어야_함()
    {
        // Arrange
        var sut = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>());

        const int GetAmount = 7;
        var drawInfos = new List<SkillAmountData>
        {
            new SkillAmountData(SkillType.태극스킬, GetAmount),
            new SkillAmountData(SkillType.흑의결속, GetAmount),
            new SkillAmountData(SkillType.거인학살자, GetAmount),
        };

        // Act
        sut.AddSkills(drawInfos);

        // Assert
        AssertSkill(SkillType.태극스킬);
        AssertSkill(SkillType.흑의결속);
        AssertSkill(SkillType.거인학살자);

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
    public void 스킬_관련_데이터를_반환해야_함(int level, bool expected)
    {
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() 
        { 
            { SkillType.태극스킬, new PlayerOwnedSkillInfo(level, 0) },
            { SkillType.컬러마스터, new PlayerOwnedSkillInfo(level, 1) },
        });
        var skillUpgradeDatas = Enumerable.Repeat(new SkillUpgradeData(), 4);
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory, SkillLevelDatas);

        Assert.AreEqual(expected, sut.SkillIsMax(SkillType.태극스킬));
        Assert.IsTrue(sut.SkillIsMax(SkillType.컬러마스터));
    }

    [Test]
    [TestCase(1, 0, 30)]
    [TestCase(1, 10, 20)]
    [TestCase(1, 30, 0)]
    [TestCase(3, 0, 24)]
    [TestCase(4, 5, 11)]
    [TestCase(5, 0, 0)]
    public void 현재_가질_수_있는_스킬의_수량을_계산해야_함(int level, int hasAmount, int expected)
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
            { SkillType.태극스킬, new PlayerOwnedSkillInfo(level, hasAmount) },
            { SkillType.컬러마스터, new PlayerOwnedSkillInfo(1, 1) },
        });
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory, SkillLevelDatas);

        var result = sut.CalculateHasableExpAmount(SkillType.태극스킬);

        Assert.AreEqual(expected, result);
        Assert.Zero(sut.CalculateHasableExpAmount(SkillType.컬러마스터));
    }

    SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() {Level = level, NeedExp = needExp};
}
