using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Debug;

public class DataChangeTester
{
    readonly int RESULT_DATA = 300;

    public DataChangeTester() => InitializerSingleton();

    void InitializerSingleton()
    {
        Managers.Multi.Init();
        var unitMA = Multi_UnitManager.Instance;
    }

    public void TestChangeAllUnitStat()
    {
        Log("유닛 전체 변경 테스트");
        SpawnUnit(0, 0);
        SpawnUnit(1, 0);
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.Damage, RESULT_DATA);
        AssertUnitStatChange(stat => stat.Damage, RESULT_DATA, x => true);
        SpawnUnit(4, 2);
        AssertUnitStatChange(stat => stat.Damage, RESULT_DATA, x => true);
    }

    public void TestChangeUnitDataWithCondition()
    {
        Log("유닛 스탯 변경 테스트!!");

        var redSwordFlag = new UnitFlags(0, 0);
        SpawnUnit(0, 0);
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.Damage, RESULT_DATA, redSwordFlag);

        AssertUnitStatChange(stat => stat.Damage, RESULT_DATA, flag => flag == redSwordFlag);
        var orange = UnitColor.Orange;
        SpawnUnit(4, 1);
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.BossDamage, RESULT_DATA);
        Multi_UnitManager.Instance.Stat.ScaleUnitStat(UnitStatType.BossDamage, 1.5f, orange);
        AssertUnitStatChange(stat => stat.BossDamage, Mathf.RoundToInt(RESULT_DATA * 1.5f), flag => flag.UnitColor == orange);
    }

    void AssertUnitStatChange(Func<UnitStat, int> getResult, int resultData, Func<UnitFlags, bool> condition)
    {
        foreach (var stat in Managers.Multi.Data.GetUnitStats(condition))
            Assert(getResult(stat) == resultData, $"DB의 값이 예상과 다름 : {getResult(stat)} != {resultData}");
        foreach (var unit in Multi_UnitManager.Instance.Master.GetUnits(0, x => condition(x.UnitFlags)))
            Assert(getResult(unit.Stat) == resultData, $"소환된 유닛 대미지가 예상과 다름 : {getResult(unit.Stat)} != {resultData}");
    }

    void SpawnUnit(int colorNum, int classNum) => Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNum, classNum));
}
