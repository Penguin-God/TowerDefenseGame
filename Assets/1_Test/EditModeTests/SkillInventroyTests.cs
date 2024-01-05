using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillInventroyTests
{
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
        const int MaxLevel = 5;
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() { { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(level, 0) } });
        var skillUpgradeDatas = Enumerable.Repeat(new SkillUpgradeData(), MaxLevel - 1);
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory);

        Assert.AreEqual(expected, sut.SkillIsMax(SkillType.�±ؽ�ų));
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
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() { { SkillType.�±ؽ�ų, new PlayerOwnedSkillInfo(level, hasAmount) } });
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory);

        var result = sut.CalculateHasableExpAmount(SkillType.�±ؽ�ų);

        Assert.AreEqual(expected, result);
    }

    SkillUpgradeData CreateUpgradeData(int level, int needExp) => new() {Level = level, NeedExp = needExp};
}
