using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillTest : MonoBehaviour
{
    Dictionary<SkillType, bool> _skillTypeByFlag = new Dictionary<SkillType, bool>();
    
    void Awake()
    {
        foreach (SkillType item in System.Enum.GetValues(typeof(SkillType)))
            _skillTypeByFlag.Add(item, false);
    }

    public void ActiveSkill(SkillType skillType)
    {
        new UserSkillShopUseCase().GetSkillExp(skillType, 1);

        _skillTypeByFlag[skillType] = true;
        var container = FindObjectOfType<BattleScene>().GetBattleContainer();
        var skill = UserSkillFactory.CreateUserSkill(skillType, container);
        container.GetMultiActiveSkillData().SetData(0, new ActiveUserSkillDataContainer(skillType, 1, skillType, 1, Managers.Data));
        skill.InitSkill();
        FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkill[] { skill });
    }
}
