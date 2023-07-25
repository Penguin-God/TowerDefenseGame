using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeteorCalculator
{
    readonly MeteorStackData _meteorScoreData;
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg;
    readonly int DefaultDamage;
    readonly int DamagePerStack;
    public CombineMeteorCalculator(MeteorStackData meteorScoreData, IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg)
    {
        _meteorScoreData = meteorScoreData;
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
                result += _meteorScoreData.GetClassStack(flag.UnitClass);
        }
        return result;
    }

    public int CalculateMeteorDamage(int score, int damagePerScore, int stack, int damagePerStack)
    {
        int result = 0;
        result += score * damagePerScore;
        result += stack * damagePerStack;
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
