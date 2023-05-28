using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitPassiveController
{
    public void AddYellowSwordmanCombineGold(UnitFlags flag)
    {
        var conditions = new UnitCombineSystem(Managers.Data.CombineConditionByUnitFalg).GetNeedFlags(flag);
        foreach (var item in conditions.Where(x => x == new UnitFlags(2, 0)))
            Multi_GameManager.Instance.AddGold(Multi_GameManager.Instance.BattleData.YellowKnightRewardGold);
    }
}
