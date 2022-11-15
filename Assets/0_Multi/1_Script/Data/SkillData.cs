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

[System.Serializable]
public class UserSkillGoodsData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] int _level;
    [SerializeField] UserSkillClass _skillClass;
    [SerializeField] MoneyType _moneyType;
    [SerializeField] int _price;
    [SerializeField] string _skillName;
    [SerializeField] string _description;
    [SerializeField] string _imagePath;
    [SerializeField] UserSkillLevelData[] _levelDatas;

    public SkillType SkillType => _skillType;
    public int Level => _level;
    public UserSkillClass SkillClass => _skillClass;
    public UserSkillMetaData MetaData => new UserSkillMetaData(_skillType, _level);
    public MoneyType MoneyType => _moneyType;
    public int Price => _price;
    public string SkillName => _skillName;
    public string Description => _description;
    public string ImagePath => _imagePath;
    public UserSkillLevelData[] LevelDatas => _levelDatas;
    public void SetLevelDatas(UserSkillLevelData[] newLevelDatas) => _levelDatas = newLevelDatas;
}

[System.Serializable]
public class UserSkillLevelData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] int _level;
    [SerializeField] int _price;
    [SerializeField] int _exp;
    [SerializeField] float[] _battleDatas;

    public SkillType SkillType => _skillType;
    public int Level => _level;
    public int Price => _price;
    public int Exp => _exp;
    public float[] BattleDatas => _battleDatas;
}

public class UserSkillGoodsLoder : ICsvLoader<UserSkillMetaData, UserSkillGoodsData>
{
    public Dictionary<UserSkillMetaData, UserSkillGoodsData> MakeDict(string csv)
    {
        var skillDatas = CsvUtility.CsvToList<UserSkillGoodsData>(csv);
        var skillLevelDatas = LoadLevleData("SkillData/SkillLevelData");
        Debug.Log(skillLevelDatas[0].SkillType);
        foreach (var item in skillDatas)
            item.SetLevelDatas(skillLevelDatas.Where(x => x.SkillType == item.SkillType).ToArray());
        return skillDatas.ToDictionary(x => new UserSkillMetaData(x.SkillType, x.Level), x => x);
    }

    UserSkillLevelData[] LoadLevleData(string path)
        => CsvUtility.CsvToArray<UserSkillLevelData>(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}").text).ToArray();
}
