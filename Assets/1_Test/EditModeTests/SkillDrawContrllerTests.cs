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
        TestPersistence persistence = new();
        var skillDatas = new UserSkill[]
        {
            CreateSkill(  SkillType.�±ؽ�ų, UserSkillClass.Main),
            CreateSkill(  SkillType.���ǰ��, UserSkillClass.Main),
            CreateSkill(  SkillType.���۰������, UserSkillClass.Sub),
            CreateSkill(  SkillType.�����л���, UserSkillClass.Sub),
        };
        SkillDrawer skillDrawer = new(skillDatas);
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };

        // Act
        var sut = new SkillDrawUseCase(skillDrawer, playerDataManager, persistence);

        // Assert
        foreach (var resultInfo in sut.DrawSkills(drawInfos))
        {
            Assert.IsTrue(playerDataManager.SkillInventroy.HasSkill(resultInfo.SkillType));
            var skillData = playerDataManager.SkillInventroy.GetSkillInfo(resultInfo.SkillType);
            Assert.AreEqual(skillData.HasAmount, resultInfo.Amount);
        }
        
        // ���Ӽ� ������ ����Ǿ������� üũ
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(PlayerDataManager playerData) => IsExecute = true;
}
