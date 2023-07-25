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

public class CombineMeteorCalculator
{
    readonly MeteorStackData _meteorStackData;
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg;
    readonly int DefaultDamage;
    readonly int DamagePerStack;
    public CombineMeteorCalculator(MeteorStackData meteorScoreData, IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg)
    {
        _meteorStackData = meteorScoreData;
        _combineConditionByUnitFalg = combineConditionByUnitFalg;
    }
    
    public CombineMeteorCalculator(IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg, int defaultDamage, int damagePerStack)
    {
        _combineConditionByUnitFalg = combineConditionByUnitFalg;
        DamagePerStack = damagePerStack;
        DefaultDamage = defaultDamage;
    }
    
    public int CalculateMeteorStack(UnitFlags combineUnitFlag)
    {
        int result = 0;
        foreach (var flag in new UnitCombineSystem(_combineConditionByUnitFalg).GetNeedFlags(combineUnitFlag))
        {
            if (flag.UnitColor == UnitColor.Red)
                result += _meteorStackData.GetClassStack(flag.UnitClass);
        }
        return result;
    }

    public int CalculateMeteorDamage(int stack) => DefaultDamage + (stack * DamagePerStack);
}

public struct MeteorStackData
{
    readonly int SwordmanScore;
    readonly int ArcherScore;
    readonly int SpearmanScore;
    
    public MeteorStackData(int swordmanScore, int archerScore, int spearmanScore)
    {
        SwordmanScore = swordmanScore;
        ArcherScore = archerScore;
        SpearmanScore = spearmanScore;
    }

    public int GetClassStack(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Swordman: return SwordmanScore;
            case UnitClass.Archer: return ArcherScore;
            case UnitClass.Spearman: return SpearmanScore;
            default: return 0;
        }
    }
}
