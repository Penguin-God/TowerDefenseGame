using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EquipSkillManager
{
    Dictionary<UserSkillClass, SkillType> _typeByClass = new Dictionary<UserSkillClass, SkillType>()
    {
        {UserSkillClass.Main, SkillType.None},
        {UserSkillClass.Sub, SkillType.None},
    };
    public IEnumerable<SkillType> EquipSkills => _typeByClass.Values;

    public SkillType MainSkill => _typeByClass[UserSkillClass.Main];
    public SkillType SubSkill => _typeByClass[UserSkillClass.Sub];

    public event Action<UserSkillClass, SkillType> OnEquipSkillChanged = null;

    public void ChangedEquipSkill(UserSkillClass skillClass, SkillType skillType)
    {
        _typeByClass[skillClass] = skillType;
        OnEquipSkillChanged?.Invoke(skillClass, skillType);
    }

    public void AllUnEquip()
    {
        ChangedEquipSkill(UserSkillClass.Main, SkillType.None);
        ChangedEquipSkill(UserSkillClass.Sub, SkillType.None);
    }
}

public class Multi_ClientData
{
    #region Money 구조체
    [Serializable]
    public class Money
    {
        public string Name;
        public int Id;
        public const string path = "MoneyData";

        public int Amount;

        public void SetAmount(int newAmount)
        {
            Amount = newAmount;
        }
    }
    #endregion

    Dictionary<MoneyType, Money> moneyByType = new Dictionary<MoneyType, Money>();
    public IReadOnlyDictionary<MoneyType, Money> MoneyByType => moneyByType;


    EquipSkillManager _equipSkillManager = new EquipSkillManager();
    public EquipSkillManager EquipSkillManager => _equipSkillManager;


    Dictionary<SkillType, int> _skillByLevel = new Dictionary<SkillType, int>();
    public IEnumerable<SkillType> HasSkills => _skillByLevel.Where(x => x.Value > 0).Select(x => x.Key);
    public UserSkillLevelData GetSkillLevelData(SkillType skillType) => Managers.Data.UserSkill.GetSkillLevelData(skillType, _skillByLevel[skillType]);

    Dictionary<SkillType, int> _skillByExp = new Dictionary<SkillType, int>();
    public Dictionary<SkillType, int> SkillByExp => _skillByExp;
    public void GetExp(SkillType skill, int getQuantity)
    {
        if (_skillByLevel[skill] == 0)
            _skillByLevel[skill]++;

        _skillByExp[skill] += getQuantity;
    }

    public bool UpgradeSkill(SkillType skill)
    {
        if (CanUpgrade(skill) == false) return false;

        _skillByExp[skill] -= GetSkillLevelData(skill).Exp;
        _skillByLevel[skill]++;
        return true;
    }

    bool CanUpgrade(SkillType skill)
        => moneyByType[Managers.Data.UserSkill.GetSkillGoodsData(skill).MoneyType].Amount >= GetSkillLevelData(skill).Price
           && _skillByExp[skill] >= GetSkillLevelData(skill).Exp;

    public void Init()
    {
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            if (type == SkillType.None) continue;
            _skillByLevel.Add(type, 0);
            _skillByExp.Add(type, 0);
        }

        List<Money> moneyData = CsvUtility.CsvToArray<Money>(Resources.Load<TextAsset>("Data/ClientData/MoneyData").text).ToList();
        moneyByType = moneyData.ToDictionary(x => (MoneyType)Enum.ToObject(typeof(MoneyType), x.Id), x => x);
    }
}

public enum SkillType
{
    None,
    시작골드증가,
    시작고기증가,
    최대유닛증가,
    태극스킬,
    검은유닛강화,
    노란기사강화,
    컬러마스터,
    상대색깔변경,
    고기혐오자,
    판매보상증가,
    보스데미지증가,
}

public enum MoneyType
{
    Iron = 1,
    Wood = 2,
    Hammer = 3,
}

public enum UserSkillClass
{
    Main,
    Sub,
}

public class UserSkillShopUseCase
{
    public void GetSkillExp(SkillType skillType, int getQuantity)
    {
        if (skillType == SkillType.None) return;
        Managers.ClientData.GetExp(skillType, getQuantity);
    }

    public void BuyUserSkill(SkillType skillType)
    {
        var skillData = Managers.Data.UserSkill.GetSkillGoodsData(skillType);
        // TODO : 나중에 가격 정하기
        if (Managers.ClientData.MoneyByType[skillData.MoneyType].Amount >= 10)
        {
            Managers.ClientData.MoneyByType[skillData.MoneyType].Amount -= 10;

            GetSkillExp(skillType, 1);
        }
    }
}

class MoneyPresenter
{
    public string GetKoreaText(MoneyType moneyType)
    {
        switch (moneyType)
        {
            case MoneyType.Iron: return "철";
            case MoneyType.Wood: return "나무";
            case MoneyType.Hammer: return "망치";
        }

        return "";
    }
}