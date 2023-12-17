using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    UserSkill CreateSkill(SkillType skill, UserSkillClass _class) => new (skill, _class);

    [Test]
    public void 뽑기를_하면_유저_데이터가_갱신되고_영속성_저장도_해야_됨()
    {
        // Arrange
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()), 0, 0);
        TestPersistence persistence = new();
        var skillDatas = new UserSkill[]
        {
            CreateSkill(  SkillType.태극스킬, UserSkillClass.Main),
            CreateSkill(  SkillType.흑의결속, UserSkillClass.Main),
            CreateSkill(  SkillType.시작골드증가, UserSkillClass.Sub),
            CreateSkill(  SkillType.거인학살자, UserSkillClass.Sub),
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
        
        // 영속성 저장은 실행되었는지만 체크
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(PlayerDataManager playerData) => IsExecute = true;
}
