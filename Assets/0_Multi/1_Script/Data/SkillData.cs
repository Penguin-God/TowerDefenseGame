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

public struct UserSkillGoodsData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] int _level;
    [SerializeField] MoneyType _moneyType;
    [SerializeField] int _price;
    [SerializeField] string _skillName;
    [SerializeField] string _description;
    [SerializeField] string _imagePath;

    public SkillType SkillType => _skillType;
    public int Level => _level;
    public UserSkillMetaData MetaData => new UserSkillMetaData(_skillType, _level);
    public MoneyType MoneyType => _moneyType;
    public int Price => _price;
    public string SkillName => _skillName;
    public string Description => _description;
    public string ImagePath => _imagePath;
}

public class UserSkillGoodsLoder : ICsvLoader<UserSkillMetaData, UserSkillGoodsData>
{
    public Dictionary<UserSkillMetaData, UserSkillGoodsData> MakeDict(string csv)
        => CsvUtility.CsvToArray<UserSkillGoodsData>(csv).ToDictionary(x => new UserSkillMetaData(x.SkillType, x.Level), x => x);
}
