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

    public void TestChangeMultiUnitData()
    {
        Log("멀티 유닛 데이터 변경 테스트");
        var multi = new MultiManager();
        multi.Init();
        var multiDataManager = multi.Data;
        multiDataManager.ChangeUnitStat(UnitStatType.Damage, RESULT_DATA);
        foreach (var stat in multiDataManager.GetUnitStats(flag => true))
            Assert(stat.Damage == RESULT_DATA);
    }

    public void TestUnitDataChangeFacade()
    {
        Log("멀티 유닛 데이터 퍼사드 변경 테스트");
        var multi = new MultiManager();
        multi.Init();
        var facade = multi.Instantiater.PhotonInstantiate("RPCObjects/RPCGameObject", Vector2.zero).AddComponent<UnitStatChangeFacade>();
        var multiDataManager = multi.Data;
        facade.Init(multiDataManager, Multi_UnitManager.Instance);

        facade.ChangeUnitStat(UnitStatType.Damage, RESULT_DATA);
        foreach (var stat in multiDataManager.GetUnitStats(flag => true))
            Assert(stat.Damage == RESULT_DATA);
    }
}
