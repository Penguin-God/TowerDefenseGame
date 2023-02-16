using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Debug;

public class DataChangeTester
{
    readonly int RESULT_DATA = 300;

    public void TestChangeAllUnitStat()
    {
        Log("유닛 전체 변경 테스트");
        var unitMA = Multi_UnitManager.Instance; // Init위해서 일부러 이러는 거임
        SpawnUnit(0, 0);
        SpawnUnit(1, 0);

        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.Damage, RESULT_DATA);

        foreach (var stat in Managers.Multi.Data.GetUnitStats(flag => true))
            Assert(stat.Damage == RESULT_DATA, "DB의 값이 예상과 다름");
        foreach (var unit in Multi_UnitManager.Instance.Master.GetUnits(0))
            Assert(unit.Damage == RESULT_DATA, "소환된 유닛 대미지가 예상과 다름");

        SpawnUnit(4, 2);

    }

    public void TestChangeUnitData()
    {
        Log("유닛 스탯 변경 테스트!!");

        var redSwordFlag = new UnitFlags(0, 0);
        SpawnUnit(0, 0);
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.Damage, RESULT_DATA, redSwordFlag);

        foreach (var stat in Managers.Multi.Data.GetUnitStats(flag => flag == redSwordFlag))
            Assert(stat.Damage == RESULT_DATA);
        foreach (var unit in Multi_UnitManager.Instance.Master.GetUnits(0, unit => unit.UnitFlags == redSwordFlag))
            Assert(unit.Damage == RESULT_DATA);

        var orange = UnitColor.Orange;
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.BossDamage, RESULT_DATA, orange);
        Multi_UnitManager.Instance.Stat.SetUnitStat(UnitStatType.BossDamage, 1.5f, orange);

        foreach (var stat in Managers.Multi.Data.GetUnitStats(flag => unit => unit.unitColor == orange))
            Assert(stat.BossDamage == Mathf.RoundToInt(RESULT_DATA * 1.5f));
        foreach (var unit in Multi_UnitManager.Instance.Master.GetUnits(0, unit => unit.unitColor == orange))
            Assert(unit.BossDamage == Mathf.RoundToInt(RESULT_DATA * 1.5f));
    }

    void SpawnUnit(int colorNum, int classNum) => Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNum, classNum));
}
