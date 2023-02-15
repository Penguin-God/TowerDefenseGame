using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Debug;

public class DataChangeTester
{
    readonly int RESULT_DATA = 300;
    DataManager.UnitData GetUnitData()
    {
        var dataManager = new DataManager();
        dataManager.Init();
        return dataManager.Unit;
    }

    public void TestChangeUnitAllData()
    {
        Log("유닛 스탯 전체 변경 테스트!!");
        var _unitData = GetUnitData();
        _unitData.ChangeAllUnitStat(stat => stat.SetBossDamage(RESULT_DATA));
        foreach (var stat in _unitData.UnitStatByFlag.Values)
            Assert(stat.BossDamage == RESULT_DATA);
    }

    public void TestChangeUnitData()
    {
        Log("유닛 스탯 변경 테스트!!");
        var _unitData = GetUnitData();
        _unitData.ChangeUnitStat(IsRedUnit, stat => stat.SetDamage(RESULT_DATA));        
        foreach (var stat in _unitData.UnitStatByFlag.Values.Where(x => IsRedUnit(x.Flag)))
            Assert(stat.Damage == RESULT_DATA);

        bool IsRedUnit(UnitFlags flag) => flag.UnitColor == UnitColor.Red;
    }
}
