using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_UserSkill : TutorialController
{
    readonly int BLUE_NUMBER = 2;
    readonly int YELLOW_NUMBER = 3;

    protected override void Init()
    {
        Managers.ClientData.GetExp(SkillType.태극스킬, 1);
        Managers.ClientData.GetExp(SkillType.판매보상증가, 1);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.태극스킬);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.판매보상증가);
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillInitializer().InitUserSkill());
        ChangeMaxUnitSummonColor(BLUE_NUMBER);
    }

    protected override void AddTutorials()
    {
        
        AddActionCommend(() => ChangeMaxUnitSummonColor(YELLOW_NUMBER));
    }

    protected override bool TutorialStartCondition() => CheckOnTeaguke();

    bool CheckOnTeaguke() => Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(0, 0)] >= 1
            && Multi_UnitManager.Instance.UnitCountByFlag[new UnitFlags(1, 0)] >= 1;

    void ChangeMaxUnitSummonColor(int colorNumber)
        => Multi_GameManager.instance.BattleData.UnitSummonData.maxColorNumber = colorNumber;

    void OnDestroy()
    {
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Main, SkillType.None);
        Managers.ClientData.EquipSkillManager.ChangedEquipSkill(UserSkillClass.Sub, SkillType.None);
    }
}
