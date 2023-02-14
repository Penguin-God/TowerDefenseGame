using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Debug;

public class DataChangeTester
{
    public void TestChangeUnitAllData()
    {
        Log("유닛 스탯 전체 변경 테스트!!");
        var dataManager = new DataManager();
        dataManager.Init();
        var _unitData = dataManager.Unit;

        var result = _unitData.UnitStatByFlag.Values.Select(x => x.BossDamage).ToList();
        for (int i = 0; i < result.Count; i++)
            result[i] = result[i] *= 2;
        _unitData.ChangeAllUnitStat(stat => stat.SetBossDamage(stat.BossDamage * 2));

        for (int i = 0; i < result.Count; i++)
            Assert(result[i] == _unitData.UnitStatByFlag.Values.ToList()[i].BossDamage);
    }

    public void TestChangeUnitData()
    {
        Log("유닛 스탯 변경 테스트!!");
        int testData = 300;
        var dataManager = new DataManager();
        dataManager.Init();
        var _unitData = dataManager.Unit;

        var result = _unitData.UnitStatByFlag.Values.Where(x => x.Flag.UnitColor == UnitColor.Red).Select(x => x.BossDamage).ToList();
        for (int i = 0; i < result.Count; i++)
            result[i] = testData;
        _unitData.ChangeUnitStat(x => x.UnitColor == UnitColor.Red, stat => stat.SetDamage(testData));

        for (int i = 0; i < result.Count; i++)
            Assert(result[i] == _unitData.UnitStatByFlag.Values.ToList()[i].Damage);
    }
}
