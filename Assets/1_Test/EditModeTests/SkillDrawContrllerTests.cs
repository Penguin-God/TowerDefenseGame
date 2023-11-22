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
        PlayerDataManager playerDataManager = new();
        TestPersistence persistence = new();
        var sut = new SkillDrawUseCase(null, playerDataManager, persistence);

        var result = sut.DrawSkills(null).FirstOrDefault();

        
        Assert.IsTrue(playerDataManager.UserInfo.SkillDatas.Any(x => x.SkillType == result.SkillType));
        var skillData = playerDataManager.UserInfo.SkillDatas.FirstOrDefault();
        Assert.AreEqual(skillData.SkillType, result.SkillType);
        Assert.AreEqual(skillData.HasAmount, result.Amount);

        // 영속성 저장 실행되었는지만 체크
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(UserInfo userInfo) => IsExecute = true;
}
