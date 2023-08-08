using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineMeteorStackManager
{
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg;
    readonly MeteorStackData _meteorStackData;
    
    public CombineMeteorStackManager(IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg, MeteorStackData meteorStackData)
    {
        _combineConditionByUnitFalg = combineConditionByUnitFalg;
        _meteorStackData = meteorStackData;
    }

    public int CurrentStack { get; private set; }

    public void AddCombineStack(UnitFlags combineUnitFlag)
    {
        int addStack = new UnitCombineSystem(_combineConditionByUnitFalg).GetNeedFlags(combineUnitFlag)
            .Where(x => x.UnitColor == UnitColor.Red)
            .Sum(x => _meteorStackData.GetClassStack(x.UnitClass));
        CurrentStack += addStack;
    }
}

public struct MeteorStackData
{
    readonly int SwordmanStack;
    readonly int ArcherStack;
    readonly int SpearmanStack;
    
    public MeteorStackData(int swordmanStack, int archerStack, int spearmanStack)
    {
        SwordmanStack = swordmanStack;
        ArcherStack = archerStack;
        SpearmanStack = spearmanStack;
    }

    public int GetClassStack(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Swordman: return SwordmanStack;
            case UnitClass.Archer: return ArcherStack;
            case UnitClass.Spearman: return SpearmanStack;
            default: return 0;
        }
    }
}
