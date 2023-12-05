using System.Collections;
using System.Collections.Generic;

public class UserSkillInitializer
{
    public IEnumerable<UserSkillController> InitUserSkill(BattleDIContainer container, SkillBattleDataContainer skillData)
    {
        List<UserSkillController> userSkills = new List<UserSkillController>();
        foreach (var skillType in skillData.AllSKills)
        {
            if (skillType == SkillType.None)
                continue;

            var userSkill = new UserSkillActor().ActiveSkill(skillType, container);
            if (userSkill != null)
                userSkills.Add(userSkill);
        }
        return userSkills;
    }

    public void AddSkillDependency(BattleDIContainer container, SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.��������:
                container.AddComponent<SkillColorChanger>();
                container.Inject<SkillColorChanger>(); break;
            case SkillType.���׿�:
                container.AddService(new SkillMeteorController(container.GetComponent<MeteorController>(), container.GetService<MonsterManagerController>())); break;
            case SkillType.��: container.AddComponent<TrapCreator>(); break;
        }
    }
}
