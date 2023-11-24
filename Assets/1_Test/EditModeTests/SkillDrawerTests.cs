using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillDrawerTests
{
    [Test]
    public void 뽑은_스킬과_수량은_등급과_범위_내에서_나와야_함()
    {
        for (int i = 0; i < 100; i++)
        {
            AssertSkillDraw();
        }
    }

    void AssertSkillDraw()
    {
        // Arrange
        var skillDatas = new Dictionary<UserSkillClass, IReadOnlyList<SkillType>>()
        {
            {  UserSkillClass.Main, new SkillType[]{ SkillType.흑의결속, SkillType.태극스킬} },
            {  UserSkillClass.Sub, new SkillType[]{ SkillType.시작골드증가, SkillType.거인학살자} }
        };
        SkillDrawer drawer = new(skillDatas);
        var drawInfos = new List<SkillDrawInfo>
        {
            new SkillDrawInfo(UserSkillClass.Main, 1, 10),
            new SkillDrawInfo(UserSkillClass.Main, 20, 40),
            new SkillDrawInfo(UserSkillClass.Sub, 30, 80),
        };

        // Act
        var drawnSkills = drawer.DrawSkills(drawInfos).ToArray();

        // Assert
        Assert.AreEqual(3, drawnSkills.Select(x => x.SkillType).Distinct().Count());
        CollectionAssert.Contains(skillDatas[UserSkillClass.Main], drawnSkills[0].SkillType);
        CollectionAssert.Contains(skillDatas[UserSkillClass.Main], drawnSkills[1].SkillType);
        CollectionAssert.Contains(skillDatas[UserSkillClass.Sub], drawnSkills[2].SkillType);

        Assert.That(drawnSkills[0].Amount, Is.InRange(1, 10));
        Assert.That(drawnSkills[1].Amount, Is.InRange(20, 40));
        Assert.That(drawnSkills[2].Amount, Is.InRange(30, 80));
    }
}
