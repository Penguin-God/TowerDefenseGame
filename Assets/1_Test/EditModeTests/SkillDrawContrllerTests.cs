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
        PlayerDataManager playerDataManager = new(new SkillInventroy(new Dictionary<SkillType, PlayerOwnedSkillInfo>()));
        TestPersistence persistence = new();
        SkillDrawer skillDrawer = new(new Dictionary<UserSkillClass, IReadOnlyList<SkillType>>() { { UserSkillClass.Main, new SkillType[] { SkillType.���ǰ�� } } });
        var sut = new SkillDrawUseCase(skillDrawer, playerDataManager, persistence);

        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
        };
        var result = sut.DrawSkills(drawInfos).FirstOrDefault();

        
        Assert.IsTrue(playerDataManager.SkillInventroy.HasSkill(result.SkillType));
        var skillData = playerDataManager.SkillInventroy.GetSkillInfo(result.SkillType);
        Assert.AreEqual(skillData.HasAmount, result.Amount);

        // ���Ӽ� ������ ����Ǿ������� üũ
        Assert.IsTrue(persistence.IsExecute);
    }
}

public class TestPersistence : IDataPersistence
{
    public bool IsExecute = false;
    public void Save(PlayerDataManager playerData) => IsExecute = true;
}
