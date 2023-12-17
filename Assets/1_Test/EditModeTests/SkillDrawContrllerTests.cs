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
        var drawInfos = new List<SkillAmountData>
        {
            new SkillAmountData(SkillType.�±ؽ�ų, GetAmount),
            new SkillAmountData(SkillType.���ǰ��, GetAmount),
            new SkillAmountData(SkillType.�����л���, GetAmount),
        };

        // Act
        playerDataManager.AddSkills(drawInfos);

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
