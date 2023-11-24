using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerDataManager
{
    public SkillInventroy SkillInventroy { get; private set; }
    public PlayerDataManager(SkillInventroy skillInventroy)
    {
        SkillInventroy = skillInventroy;
    }
}
