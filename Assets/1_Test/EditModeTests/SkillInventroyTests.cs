using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillInventroyTests
{
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
        const int MaxLevel = 5;
        var inventory = new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>() { { SkillType.태극스킬, new PlayerOwnedSkillInfo(level, 0) } });
        var skillUpgradeDatas = Enumerable.Repeat(new SkillUpgradeData(), MaxLevel - 1);
        var sut = new SkillDataGetter(skillUpgradeDatas, inventory);

        Assert.AreEqual(expected, sut.SkillIsMax(SkillType.태극스킬));
    }

    [Test]
    public void 최대로_가질_수_있는_스킬의_수량을_계산해야_함()
    {

    }
}
