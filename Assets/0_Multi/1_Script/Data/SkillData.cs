using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct UserSKillData
{
    [SerializeField] KeyValuePair<SkillType, int> _skillLevelPair;
    [SerializeField] float[] _datas;

    public KeyValuePair<SkillType, int> SkillLevelPair => _skillLevelPair;
    public float[] Datas => _datas;
}

public class UserSkillLoder : ICsvLoader<KeyValuePair<SkillType, int>, float[]>
{
    public Dictionary<KeyValuePair<SkillType, int>, float[]> MakeDict(string csv)
        => CsvUtility.CsvToArray<UserSKillData>(csv).ToDictionary(x => x.SkillLevelPair, x => x.Datas);
}