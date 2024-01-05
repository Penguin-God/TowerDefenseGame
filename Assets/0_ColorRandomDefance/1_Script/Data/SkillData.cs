using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct UserSkillData
{
    [SerializeField] SkillType _skillType;
    [SerializeField] UserSkillClass _skillClass;

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

    public SkillType SkillType => _skillType;
    public UserSkillClass SkillClass => _skillClass;
    public string SkillName => _skillName;
    public string Description => _description.Replace('+', ',').Replace("\\n", "\n");
    public string ImageName => _imagePath;
    public Sprite ImageSprite => Managers.Resources.Load<Sprite>(_imagePath);
    public IReadOnlyList<string> StatInfoFraems => _statInfoFraems;
}

[Serializable]
public struct UserSkillLevelData
{
    public SkillType SkillType;
    public int Level;
    public float[] BattleDatas;
}

public class UserSkillGoodsLoder : ICsvLoader<SkillType, UserSkillGoodsData>
{
    public Dictionary<SkillType, UserSkillGoodsData> MakeDict(string csv)
    {
        var skillDatas = CsvUtility.CsvToList<UserSkillGoodsData>(csv);
        var skillLevelDatas = LoadLevleData("SkillData/SkillBattleData");
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
    public int Level;
    public int NeedExp;
    public int NeedGold;
}

[Serializable]
public struct SkillLevelData
{
    public SkillType SkillType;
    public int MinLevel;
    public int MaxLevel;
}