using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    [Test]
    public void 뽑기를_하면_유저_데이터가_갱신되고_영속성_저장도_해야_됨()
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
}
