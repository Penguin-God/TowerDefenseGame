using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;
using System;

public class PathBuildersTester
{
    public void TestBuildUnitPath()
    {
        Log("유닛 패스 생성 테스트!!");
        var builder = new ResourcesPathBuilder();
        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                Assert(Resources.Load<GameObject>($"Prefabs/{builder.BuildUnitPath(new UnitFlags(color, unitClass))}") != null);
        }
    }

    public void TestBuildMonstersPath()
    {
        Log("몬스터 패스 생성 테스트!!");
        var builder = new ResourcesPathBuilder();
        var monsterNames = new string[] { "Archer", "Mage", "Spearman", "Swordman" };
        for (int i = 0; i < monsterNames.Length; i++)
            Assert(builder.BuildMonsterPath(i) == $"Enemy/Normal/Enemy_{monsterNames[i]} 1");

        for (int i = 0; i < monsterNames.Length; i++)
            Assert(builder.BuildBossMonsterPath(i) == $"Enemy/Boss/Boss_Enemy_{monsterNames[i]} 1");

        for (int i = 1; i < 7; i++)
            Assert(builder.BuildEnemyTowerPath(i) == $"Enemy/Tower/Lvl{i}_Twoer");
    }

    public void TestBuildWeaponPath()
    {
        Log("무기 패스 생성 테스트!!");
        var builder = new ResourcesPathBuilder();
        foreach (UnitColor color in Enum.GetValues(typeof(UnitColor)))
        {
            foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
                Assert(Resources.Load<GameObject>($"Prefabs/{builder.BuildUnitWeaponPath(new UnitFlags(color, unitClass))}") != null);
        }
    }
}
