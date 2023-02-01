﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;

public class PresentersTester
{
    public void TestGenerateColorChangeResultText()
    {
        Log("색깔 변경 프레젠터 테스트!!");
        var presenter = new UnitColorChangeTextPresenter();
        var beforeFlag = new UnitFlags(1, 2);
        var afterFlag = new UnitFlags(0, 2);
        Assert(presenter.GenerateColorChangeResultText(beforeFlag, afterFlag) == "파란 창병이 빨간 창병으로 변경되었습니다");
        Assert(presenter.GenerateTextShowToDisruptor(beforeFlag, afterFlag) == "스킬 사용으로 상대방의\n파란 창병이 빨간 창병으로 변경되었습니다");
        Assert(presenter.GenerateTextShowToVictim(beforeFlag, afterFlag) == "상대방의 스킬 사용으로 보유 중인\n파란 창병이 빨간 창병으로 변경되었습니다");
    }

    public void TestBuildUnitSpawnPath()
    {
        Log("유닛 패스 생성 테스트!!");
        var builder = new UnitPathBuilder();
        Assert(builder.BuildUnitPath(new UnitFlags(1, 0)) == "Unit/Swordman/Blue_Swordman 1");
        Assert(builder.BuildUnitPath(new UnitFlags(3, 3)) == "Unit/Mage/Green_Mage 1");
        Assert(builder.BuildUnitPath(new UnitFlags(6, 1)) == "Unit/Archer/White_Archer 1");
    }

    public void TestBuildMonstersSpawnPath()
    {
        Log("몬스터 패스 생성 테스트!!");
        var builder = new SpawnPathBuilder();
        var monsterNames = new string[] { "Archer", "Mage", "Spearman", "Swordman" };
        for (int i = 0; i < monsterNames.Length; i++)
            Assert(builder.BuildMonsterPath(i) == $"Enemy/Normal/Enemy_{monsterNames[i]} 1");

        for (int i = 0; i < monsterNames.Length; i++)
            Assert(builder.BuildBossMonsterPath(i) == $"Enemy/Boss/Boss_Enemy_{monsterNames[i]} 1");

        for (int i = 1; i < 7; i++)
            Assert(builder.BuildEnemyTowerPath(i) == $"Enemy/Tower/Lvl{i}_Twoer");
    }
}