using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    [Test]
    public void �̱⸦_�ϸ�_����_�����Ͱ�_���ŵǰ�_���Ӽ�_���嵵_�ؾ�_��()
    {
        // Arrange
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()));
        TestPersistence persistence = new();
        var skillDatas = new Dictionary<UserSkillClass, IReadOnlyList<SkillType>>()
        {
            {  UserSkillClass.Main, new SkillType[]{ SkillType.���ǰ��, SkillType.�±ؽ�ų} },
            {  UserSkillClass.Sub, new SkillType[]{ SkillType.���۰������, SkillType.�����л���} }
        };
        SkillDrawer skillDrawer = new SkillDrawer(skillDatas);
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
