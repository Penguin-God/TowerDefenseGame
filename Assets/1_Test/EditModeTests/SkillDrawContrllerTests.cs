using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    [Test]
    public void �̱⸦_�ϸ�_����_�����Ͱ�_���ŵǰ�_���Ӽ�_���嵵_�ؾ�_��()
    {
        // Arrange
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()), 0, 0);

        const int GetAmount = 7;
        var drawInfos = new List<SkillDrawResultInfo>
        {
            new SkillDrawResultInfo(SkillType.�±ؽ�ų, GetAmount),
            new SkillDrawResultInfo(SkillType.���ǰ��, GetAmount),
            new SkillDrawResultInfo(SkillType.�����л���, GetAmount),
        };

        // Act
        var sut = new SkillDrawUseCase(drawInfos);
        sut.GiveProduct(playerDataManager);

        // Assert
        AssertSkill(SkillType.�±ؽ�ų);
        AssertSkill(SkillType.���ǰ��);
        AssertSkill(SkillType.�����л���);

        void AssertSkill(SkillType skillType)
        {
            Assert.IsTrue(playerDataManager.SkillInventroy.HasSkill(skillType));
            var skillData = playerDataManager.SkillInventroy.GetSkillInfo(skillType);
            Assert.AreEqual(skillData.HasAmount, GetAmount);
        }
    }
}
