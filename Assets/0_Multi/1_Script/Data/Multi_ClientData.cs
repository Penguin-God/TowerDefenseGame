﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public List<Skill> EquipSkills = new List<Skill>();

    public void Init()
    {
        List<Skill> playerDatas = CsvUtility.GetEnumerableFromCsv<Skill>(Resources.Load<TextAsset>("Data/ClientData/SkillData").text).ToList();
        Debug.Log(playerDatas.Count);
        playerDatas.ForEach(x => Debug.Log(x.Name));
        skillByType = playerDatas.ToDictionary(x => (SkillType)Enum.ToObject(typeof(SkillType), x.Id), x => x);

        List<Money> moneyData = CsvUtility.GetEnumerableFromCsv<Money>(Resources.Load<TextAsset>("Data/ClientData/MoneyData").text).ToList();
        moneyByType = moneyData.ToDictionary(x => (MoneyType)Enum.ToObject(typeof(MoneyType), x.Id), x => x);
    }

    public void SaveData<T>(string path)
    {
        if (typeof(T) == typeof(Skill)) SaveData(skillByType.Values, path);
        else if(typeof(T) == typeof(Money)) SaveData(moneyByType.Values, path);
    }

    // TODO : 세이브 개선하기
    void SaveData<T>(IEnumerable<T> datas, string path)
    {
        string csv = CsvUtility.EnumerableToCsv(datas);
        CsvUtility.SaveCsv(csv, path);
    }
}

public enum SkillType
{
    시작골드증가 = 1,
    최대유닛증가 = 2,
    태극스킬 = 3,
    검은유닛강화 = 4,
    노란기사강화 = 5,
    상대색깔변경 = 6,
    흔한스킬 = 7,
    고기혐오자 = 8,
    판매보상증가 = 9,
    보스데미지증가 = 10,
}

public enum MoneyType
{
    Iron = 1,
    Wood = 2,
    Hammer = 3,
}