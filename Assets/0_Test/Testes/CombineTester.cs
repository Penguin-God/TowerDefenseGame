using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.Debug;

public class CombineTester : MonoBehaviour
{
    public void TestGetCombinableUnitFalgs()
    {
        print("조합 가능 유닛 가져오는거 테스트!!");
        var system = new UnitCombineSystem();
        var unitFlags = new UnitFlags[]
        {
            new UnitFlags(0,0),
            new UnitFlags(0,0),
            new UnitFlags(0,0),
            new UnitFlags(0,1),
        };
        var answer = new UnitFlags[]
        {
            new UnitFlags(1,0),
            new UnitFlags(0,5),
        };
        
        Assert(answer.SequenceEqual(system.GetCombinableUnitFalgs(unitFlags)));
        foreach (var item in system.GetCombinableUnitFalgs(unitFlags))
        {
            print($"{item.ColorNumber} : {item.ClassNumber}");
        }
    }
}
