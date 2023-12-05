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

    BattleDIContainer _container;
    public void DependencyInject(BattleDIContainer container)
    {
        _container = container;
    }

    public void ActiveSkill(SkillType skillType)
    {
        _skillTypeByFlag[skillType] = true;
        var skillInitializer = new UserSkillInitializer();

        skillInitializer.AddSkillDependency(_container, skillType); // 인터페이스 못 맞춰서 이렇게 하긴 했는데 진짜 별로임.
        var skill = new UserSkillActor().ActiveSkill(skillType, _container);

        _container.GetMultiActiveSkillData().GetData(PlayerIdManager.Id).ChangeEquipSkill(Managers.Data.UserSkill.GetSkillBattleData(skillType, 1));
        if(skill != null)
            FindObjectOfType<EffectInitializer>().SettingEffect(new UserSkillController[] { skill }, _container.GetEventDispatcher(), _container.GetService<UnitManagerController>());
    }
}
