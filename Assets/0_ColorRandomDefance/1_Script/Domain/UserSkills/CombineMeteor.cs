using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeteor
{
    readonly MeteorScoreData _meteorScoreData;
    readonly IReadOnlyDictionary<UnitFlags, CombineCondition> _combineConditionByUnitFalg;
    
    public CombineMeteor(MeteorScoreData meteorScoreData, IReadOnlyDictionary<UnitFlags, CombineCondition> combineConditionByUnitFalg)
    {
        _meteorScoreData = meteorScoreData;
        _combineConditionByUnitFalg = combineConditionByUnitFalg;
    }

    public int CalculateMeteorScore(UnitFlags combineUnitFlag)
    {
        int result = 0;
        foreach (var flag in new UnitCombineSystem(_combineConditionByUnitFalg).GetNeedFlags(combineUnitFlag))
        {
            if (flag.UnitColor == UnitColor.Red)
                result += _meteorScoreData.GetClassScore(flag.UnitClass);
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
}

public struct MeteorScoreData
{
    readonly int SwordmanScore;
    readonly int ArcherScore;
    readonly int SpearmanScore;
    
    public MeteorScoreData(int swordmanScore, int archerScore, int spearmanScore)
    {
        SwordmanScore = swordmanScore;
        ArcherScore = archerScore;
        SpearmanScore = spearmanScore;
    }

    public int GetClassScore(UnitClass unitClass)
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
