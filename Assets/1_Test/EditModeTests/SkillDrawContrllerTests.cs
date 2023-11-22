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
        PlayerDataManager playerDataManager = new();
        TestPersistence persistence = new();
        var sut = new SkillDrawUseCase(null, playerDataManager, persistence);

        var result = sut.DrawSkills(null).FirstOrDefault();

        
        Assert.IsTrue(playerDataManager.UserInfo.SkillDatas.Any(x => x.SkillType == result.SkillType));
        var skillData = playerDataManager.UserInfo.SkillDatas.FirstOrDefault();
        Assert.AreEqual(skillData.SkillType, result.SkillType);
        Assert.AreEqual(skillData.HasAmount, result.Amount);

        // ���Ӽ� ���� ����Ǿ������� üũ
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(UserInfo userInfo) => IsExecute = true;
}
