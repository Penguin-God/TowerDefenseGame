using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UserSkillInitializer
{
    public IEnumerable<UserSkillController> InitUserSkill(BattleDIContainer container, SkillBattleDataContainer skillData)
    {
        List<UserSkillController> userSkills = new ();
        userSkills.Add(new UserSkillActor().ActiveSkill(skillData.MainSkill, container));
        userSkills.Add(new UserSkillActor().ActiveSkill(skillData.SubSkill, container));
        return userSkills.Where(x => x != null);
    }

    public void AddSkillDependency(BattleDIContainer container, SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.마나변이:
                container.AddComponent<SkillColorChanger>();
                container.Inject<SkillColorChanger>(); break;
            case SkillType.메테오:
                container.AddService(new SkillMeteorController(container.GetComponent<MeteorController>(), container.GetService<MonsterManagerController>())); break;
            case SkillType.덫: container.AddComponent<TrapCreator>(); break;
        }
    }
}
