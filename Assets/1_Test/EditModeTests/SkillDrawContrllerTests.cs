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
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()), 0, 0);

        const int GetAmount = 7;
        var drawInfos = new List<SkillDrawResultInfo>
        {
            new SkillDrawResultInfo(SkillType.태극스킬, GetAmount),
            new SkillDrawResultInfo(SkillType.흑의결속, GetAmount),
            new SkillDrawResultInfo(SkillType.거인학살자, GetAmount),
        };

        // Act
        var sut = new SkillDrawUseCase(drawInfos);
        sut.GiveProduct(playerDataManager);

        // Assert
        AssertSkill(SkillType.태극스킬);
        AssertSkill(SkillType.흑의결속);
        AssertSkill(SkillType.거인학살자);

        void AssertSkill(SkillType skillType)
        {
            Assert.IsTrue(playerDataManager.SkillInventroy.HasSkill(skillType));
            var skillData = playerDataManager.SkillInventroy.GetSkillInfo(skillType);
            Assert.AreEqual(skillData.HasAmount, GetAmount);
        }
    }
}
