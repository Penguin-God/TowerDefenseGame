using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawContrllerTests
{
    [Test]
    public void 뽑기를_하면_유저_데이터가_갱신되고_영속성_저장도_해야_됨()
    {
        // Arrange
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()));
        TestPersistence persistence = new();
        var skillDatas = new Dictionary<UserSkillClass, IReadOnlyList<SkillType>>()
        {
            {  UserSkillClass.Main, new SkillType[]{ SkillType.흑의결속, SkillType.태극스킬} },
            {  UserSkillClass.Sub, new SkillType[]{ SkillType.시작골드증가, SkillType.거인학살자} }
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
        
        // 영속성 저장은 실행되었는지만 체크
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(PlayerDataManager playerData) => IsExecute = true;
}
