using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    UserSkill CreateSkill(SkillType skill, UserSkillClass _class) => new (skill, _class);

    [Test]
    public void �̱⸦_�ϸ�_����_�����Ͱ�_���ŵǰ�_���Ӽ�_���嵵_�ؾ�_��()
    {
        // Arrange
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()), 0, 0);
        var skillDatas = new UserSkill[]
        {
            CreateSkill(SkillType.�±ؽ�ų, UserSkillClass.Main),
            CreateSkill(SkillType.���ǰ��, UserSkillClass.Main),
            CreateSkill(SkillType.�����л���, UserSkillClass.Sub),
        };
        
        SkillDrawer skillDrawer = new(skillDatas);
        const int GetAmount = 7;
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, GetAmount, GetAmount),
            new SkillDrawInfo(UserSkillClass.Main, GetAmount, GetAmount),
            new SkillDrawInfo(UserSkillClass.Sub, GetAmount, GetAmount),
        };

        // Act
        var sut = new SkillDrawUseCase(skillDrawer, drawInfos);
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
