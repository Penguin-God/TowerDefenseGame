using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerSkillData
{
    public SkillType SkillType;
    public int Level;
    public int HasAmount;

    public PlayerSkillData(SkillType type, int level, int hasAmount)
    {
        SkillType = type;
        Level = level;
        HasAmount = hasAmount;
    }
}

public struct UserInfo
{
    public string Name;
    public IEnumerable<PlayerSkillData> SkillDatas;
    public int Gem;
}
