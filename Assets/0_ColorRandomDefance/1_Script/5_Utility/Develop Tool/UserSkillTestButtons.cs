using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillTestButtons : MonoBehaviour
{
    Dictionary<SkillType, bool> _skillTypeByFlag = new Dictionary<SkillType, bool>();
    
    void Awake()
    {
        foreach (SkillType item in System.Enum.GetValues(typeof(SkillType)))
            _skillTypeByFlag.Add(item, false);
    }

    public void ActiveSkill(SkillType skillType)
    {
        _skillTypeByFlag[skillType] = true;
        var container = FindObjectOfType<BattleScene>().GetBattleContainer();
        var skill = new UserSkillFactory().ActiveSkill(skillType, container);
        container.GetMultiActiveSkillData().GetData(PlayerIdManager.Id).ChangeEquipSkill(Managers.Data.UserSkill.GetSkillBattleData(skillType, 1));
        if(skill != null)
            FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkill[] { skill });
    }
}
