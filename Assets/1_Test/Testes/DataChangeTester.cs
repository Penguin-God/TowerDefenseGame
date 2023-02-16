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
        Log("유닛 전체 변경 테스트");
        var multi = Managers.Multi;
        var facade = multi.Instantiater.PhotonInstantiate("RPCObjects/RPCGameObject", Vector2.zero).AddComponent<UnitStatChangeFacade>();
        var multiDataManager = multi.Data;
        facade.Init(multiDataManager, Multi_UnitManager.Instance);

        Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(0, 0));
        Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(1, 0));

        facade.ChangeUnitStat(UnitStatType.Damage, RESULT_DATA);

        foreach (var stat in multiDataManager.GetUnitStats(flag => true))
            Assert(stat.Damage == RESULT_DATA, "DB의 값이 예상과 다름");
        Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(4, 2));

        var units = Multi_UnitManager.Instance.Master.GetUnits(0);
        Assert(units.Count() == 3);
        foreach (var unit in units)
            Assert(unit.Damage == RESULT_DATA, "소환된 유닛 대미지가 예상과 다름");
    }
}
