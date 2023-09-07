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
        var skillInitializer = new UserSkillInitializer();
        var container = FindObjectOfType<BattleScene>().GetBattleContainer();

        skillInitializer.AddSkillDependency(container, skillType); // 인터페이스 못 맞춰서 이렇게 하긴 했는데 진짜 별로임.
        var skill = new UserSkillFactory().ActiveSkill(skillType, container);

        container.GetMultiActiveSkillData().GetData(PlayerIdManager.Id).ChangeEquipSkill(Managers.Data.UserSkill.GetSkillBattleData(skillType, 1));
        if(skill != null)
            FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkill[] { skill });
    }
}
