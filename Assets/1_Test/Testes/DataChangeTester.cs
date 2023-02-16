using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Debug;

public class DataChangeTester
{
    readonly int RESULT_DATA = 300;

    public void TestChangeUnitData()
    {
        //Log("유닛 스탯 변경 테스트!!");
        //var _unitData = GetUnitData();
        //_unitData.ChangeUnitStat(IsRedUnit, stat => stat.SetDamage(RESULT_DATA));        
        //foreach (var stat in _unitData.UnitStatByFlag.Values.Where(x => IsRedUnit(x.Flag)))
        //    Assert(stat.Damage == RESULT_DATA);

        //bool IsRedUnit(UnitFlags flag) => flag.UnitColor == UnitColor.Red;
    }

    public void TestUnitDataChangeFacade()
    {
        Log("유닛 전체 변경 테스트");
        var multi = Managers.Multi;
        var facade = multi.Instantiater.PhotonInstantiate("RPCObjects/RPCGameObject", Vector2.zero).AddComponent<UnitStatChangeFacade>();
        var multiDataManager = multi.Data;
        facade.Init(multiDataManager, Multi_UnitManager.Instance);

        SpawnUnit(0, 0);
        SpawnUnit(1, 0);

        facade.ChangeUnitStat(UnitStatType.Damage, RESULT_DATA);

        foreach (var stat in multiDataManager.GetUnitStats(flag => true))
            Assert(stat.Damage == RESULT_DATA, "DB의 값이 예상과 다름");
        SpawnUnit(4, 2);

        var units = Multi_UnitManager.Instance.Master.GetUnits(0);
        Assert(units.Count() == 3);
        foreach (var unit in units)
            Assert(unit.Damage == RESULT_DATA, "소환된 유닛 대미지가 예상과 다름");
    }

    void SpawnUnit(int colorNum, int classNum) => Multi_SpawnManagers.NormalUnit.Spawn(new UnitFlags(colorNum, classNum));
}
