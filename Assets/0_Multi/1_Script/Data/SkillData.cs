using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UserSkillGoodsData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] UserSkillClass _skillClass;
    [SerializeField] MoneyType _moneyType;
    [SerializeField] string _skillName;
    [SerializeField] string _description;
    [SerializeField] string _imagePath;
    [SerializeField] UserSkillLevelData[] _levelDatas;

    public SkillType SkillType => _skillType;
    public UserSkillClass SkillClass => _skillClass;
    public MoneyType MoneyType => _moneyType;
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

public class UserSkillGoodsLoder : ICsvLoader<SkillType, UserSkillGoodsData>
{
    public Dictionary<SkillType, UserSkillGoodsData> MakeDict(string csv)
    {
        var skillDatas = CsvUtility.CsvToList<UserSkillGoodsData>(csv);
        var skillLevelDatas = LoadLevleData("SkillData/SkillLevelData");
        foreach (var item in skillDatas)
            item.SetLevelDatas(skillLevelDatas.Where(x => x.SkillType == item.SkillType).ToArray());
        return skillDatas.ToDictionary(x => x.SkillType, x => x);
    }

    UserSkillLevelData[] LoadLevleData(string path)
        => CsvUtility.CsvToArray<UserSkillLevelData>(Multi_Managers.Resources.Load<TextAsset>($"Data/{path}").text).ToArray();
}
