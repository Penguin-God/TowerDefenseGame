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
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()));
        TestPersistence persistence = new();
        SkillDrawer skillDrawer = new(new Dictionary<UserSkillClass, IReadOnlyList<SkillType>>() { { UserSkillClass.Main, new SkillType[] { SkillType.흑의결속 } } });
        var sut = new SkillDrawUseCase(skillDrawer, playerDataManager, persistence);

        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
        };
        var result = sut.DrawSkills(drawInfos).FirstOrDefault();

        
        Assert.IsTrue(playerDataManager.SkillInventroy.HasSkill(result.SkillType));
        var skillData = playerDataManager.SkillInventroy.GetSkillInfo(result.SkillType);
        Assert.AreEqual(skillData.HasAmount, result.Amount);

        // 영속성 저장은 실행되었는지만 체크
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(PlayerDataManager playerData) => IsExecute = true;
}
