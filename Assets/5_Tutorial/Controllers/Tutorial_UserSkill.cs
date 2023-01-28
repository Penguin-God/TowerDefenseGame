using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_UserSkill : TutorialController
{
    readonly int BLUE_NUMBER = 2;
    readonly int YELLOW_NUMBER = 3;
    protected override void Init() => ChangeMaxUnitSummonColor(BLUE_NUMBER);

    protected override void AddTutorials()
    {
        AddActionCommend(() => print("어"));
        AddActionCommend(() => ChangeMaxUnitSummonColor(YELLOW_NUMBER));
    }

    protected override bool TutorialStartCondition() => CheckOnTeaguke();

    bool CheckOnTeaguke() => Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 0)] >= 1
            && Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 0)] >= 1;

    void ChangeMaxUnitSummonColor(int colorNumber)
        => Multi_GameManager.instance.BattleData.UnitSummonData.maxColorNumber = colorNumber;
}
