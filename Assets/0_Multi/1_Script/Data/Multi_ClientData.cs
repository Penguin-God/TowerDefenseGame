using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SkillEquipData
{
    public SkillEquipData(UserSkillClass skillClass) => _skillClass = skillClass;

    SkillType _skillType;
    UserSkillClass _skillClass;
    bool _isEquip = false;
    public event Action<SkillEquipData> OnChangedEquipSkill = null;

    public SkillType SkillType => _skillType;
    public UserSkillClass SkillClass => _skillClass;
    public bool IsEquip => _isEquip;

    public void ChangedSkill(SkillType skillType)
    {
        _isEquip = (skillType != SkillType.None);
        _skillType = skillType;
        OnChangedEquipSkill?.Invoke(this);
    }

    public void UnEquipSkill() => ChangedSkill(SkillType.None);
}

public class EquipSkillManager
{
    public EquipSkillManager()
    {
        _mainSkillEquipData.OnChangedEquipSkill += RaiseEquipSkillChange;
        _subSkillEquipData.OnChangedEquipSkill += RaiseEquipSkillChange;
    }

    SkillEquipData _mainSkillEquipData = new SkillEquipData(UserSkillClass.Main);
    SkillEquipData _subSkillEquipData = new SkillEquipData(UserSkillClass.Sub);

    public SkillType[] EquipSkills => new[] { _mainSkillEquipData.SkillType, _subSkillEquipData.SkillType };

    public event Action<SkillEquipData> OnEquipSkillChanged = null;
    void RaiseEquipSkillChange(SkillEquipData data) => OnEquipSkillChanged?.Invoke(data);

    public void ChangedEquipSkill(UserSkillClass skillClass, SkillType skillType)
    {
        switch (skillClass)
        {
            case UserSkillClass.Main:
                _mainSkillEquipData.ChangedSkill(skillType);
                break;
            case UserSkillClass.Sub:
                _subSkillEquipData.ChangedSkill(skillType);
                break;
        }
    }

    public void AllUnEquip()
    {
        _mainSkillEquipData.UnEquipSkill();
        _subSkillEquipData.UnEquipSkill();
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

    // 추가
    Dictionary<SkillType, Skill> skillByType = new Dictionary<SkillType, Skill>();
    public IReadOnlyDictionary<SkillType, Skill> SkillByType => skillByType;

    Dictionary<MoneyType, Money> moneyByType = new Dictionary<MoneyType, Money>();
    public IReadOnlyDictionary<MoneyType, Money> MoneyByType => moneyByType;

    List<UserSkill> _equipSkills = new List<UserSkill>();
    public int EquipSkillCount => _equipSkills.Count();
    public void AddEquipSkill(SkillType type) => _equipSkills.Add(new UserSkillFactory().GetSkill(type, 1));

    Dictionary<SkillType, int> _skillByLevel = new Dictionary<SkillType, int>();
    public UserSkillLevelData GetSkillLevelData(SkillType skillType) => Multi_Managers.Data.UserSkill.GetSkillLevelData(skillType, _skillByLevel[skillType]);

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
        => moneyByType[Multi_Managers.Data.UserSkill.GetSkillGoodsData(skill).MoneyType].Amount >= GetSkillLevelData(skill).Price
           && _skillByExp[skill] >= GetSkillLevelData(skill).Exp;

    public IEnumerable<SkillType> HasSkills => _skillByLevel.Where(x => x.Value > 0).Select(x => x.Key);

    EquipSkillManager _equipSkillManager = new EquipSkillManager();
    public EquipSkillManager EquipSkillManager => _equipSkillManager;

    public void Init()
    {
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            if (type == SkillType.None) continue;
            _skillByLevel.Add(type, 0);
            _skillByExp.Add(type, 0);
        }

        List<Skill> playerDatas = CsvUtility.CsvToArray<Skill>(Resources.Load<TextAsset>("Data/ClientData/SkillData").text).ToList();
        skillByType = playerDatas.ToDictionary(x => (SkillType)Enum.ToObject(typeof(SkillType), x.Id), x => x);

        List<Money> moneyData = CsvUtility.CsvToArray<Money>(Resources.Load<TextAsset>("Data/ClientData/MoneyData").text).ToList();
        moneyByType = moneyData.ToDictionary(x => (MoneyType)Enum.ToObject(typeof(MoneyType), x.Id), x => x);
    }

    public void Clear()
    {
        foreach (var item in skillByType)
            item.Value.SetEquipSkill(false);
        _equipSkills.Clear();
    }

    // TODO : 세이브 개선하기
    //void SaveData<T>(IEnumerable<T> datas, string path)
    //{
    //    string csv = CsvUtility.CsvToArray(datas);
    //    CsvUtility.SaveCsv(csv, path);
    //}
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