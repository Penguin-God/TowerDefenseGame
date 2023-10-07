using System.Collections;
using System.Collections.Generic;

public class UserSkillInitializer
{
    public IEnumerable<UserSkill> InitUserSkill(BattleDIContainer container, SkillBattleDataContainer skillData)
    {
        List<UserSkill> userSkills = new List<UserSkill>();
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
            case SkillType.마나변이:
                container.AddComponent<SkillColorChanger>().Inject(container.GetComponent<TextShowAndHideController>()); break;
            case SkillType.메테오:
                container.AddService<SkillMeteorController>().DependencyInject(container.GetComponent<MeteorController>(), container.GetService<MonsterManagerController>(), Multi_EnemyManager.Instance);
                break;
        }
    }
}
