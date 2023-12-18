using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct UserSkillData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] UserSkillClass _skillClass;

    public SkillType SkillType => _skillType;
    public UserSkillClass SkillClass => _skillClass;
    public UserSkill CreateUserSkill() => new UserSkill(_skillType, _skillClass);
}

[Serializable]
public class UserSkillGoodsData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] UserSkillClass _skillClass;
    [SerializeField] string _skillName;
    [SerializeField] string _description;
    [SerializeField] string _imagePath;
    [SerializeField] string[] _statInfoFraems;
    [SerializeField] UserSkillLevelData[] _levelDatas;

    public SkillType SkillType => _skillType;
    public UserSkillClass SkillClass => _skillClass;
    public string SkillName => _skillName;
    public string Description => _description;
    public string ImagePath => _imagePath;
    public Sprite ImageSprite => Managers.Resources.Load<Sprite>(_imagePath);
    public IReadOnlyList<string> StatInfoFraems => _statInfoFraems;
    public UserSkillLevelData[] LevelDatas => _levelDatas;
    public void SetLevelDatas(UserSkillLevelData[] newLevelDatas) => _levelDatas = newLevelDatas;
}

[Serializable]
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
        => CsvUtility.CsvToArray<UserSkillLevelData>(Managers.Resources.Load<TextAsset>($"Data/{path}").text).ToArray();
}

public struct BattleShopGoodsData
{
    [SerializeField] string _name;
    [SerializeField] CurrencyData _priceData;
    [SerializeField] string _infoText;
    [SerializeField] GoodsData _sellData;

    public BattleShopGoodsData Clone(string name, CurrencyData priceData, string info, GoodsData goodsData)
    {
        _name = name;
        _priceData = priceData;
        _infoText = info;
        _sellData = goodsData;
        return this;
    }

    public string Name => _name;
    public CurrencyData PriceData => _priceData;
    public string InfoText => _infoText;
    public GoodsData SellData => _sellData;
}


[Serializable]
public struct SkillUpgradeData
{
    [SerializeField] int _level;
    [SerializeField] int _needExp;
    [SerializeField] int _needGold;

    public int Level => _level;
    public int NeedExp => _needExp;
    public int NeedGold => _needGold;
}