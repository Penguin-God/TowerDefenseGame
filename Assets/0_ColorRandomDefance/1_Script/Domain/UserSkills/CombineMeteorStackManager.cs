using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineMeteorStackManager
{
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg;
    readonly MeteorStackData _meteorStackData;
    readonly int NeedStackForUnitSummon;

    public CombineMeteorStackManager
        (IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg, MeteorStackData meteorStackData, int needStackForUnitSummon)
    {
        _combineConditionByUnitFalg = combineConditionByUnitFalg;
        _meteorStackData = meteorStackData;
        NeedStackForUnitSummon = needStackForUnitSummon;
    }

    public int CurrentStack { get; private set; }
    public int SummonUnitCount { get; private set; }
    int _unitSummonStackThreshold;

    public void SummonUnit() => SummonUnitCount = 0;
    public void AddCombineStack(UnitFlags combineUnitFlag)
    {
        int addStack = new UnitCombineSystem(_combineConditionByUnitFalg).GetNeedFlags(combineUnitFlag)
            .Where(x => x.UnitColor == UnitColor.Red)
            .Sum(x => _meteorStackData.GetClassStack(x.UnitClass));
        CurrentStack += addStack;

        _unitSummonStackThreshold += addStack;
        if(_unitSummonStackThreshold > NeedStackForUnitSummon)
        {
            SummonUnitCount += _unitSummonStackThreshold / NeedStackForUnitSummon;
            _unitSummonStackThreshold %= NeedStackForUnitSummon;
        }
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
