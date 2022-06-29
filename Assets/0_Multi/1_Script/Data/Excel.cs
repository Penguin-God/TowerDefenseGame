using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Excel
{
    #region Skill 구조체
    [Serializable]
    public class Skill : PlayerDataBase
    {
        public string Name;
        public int Id;
        public bool HasSkill;
        public const string path = "SkillData";

    }

    [SerializeField] public static List<Skill> Skills;
    #endregion

    #region Money 구조체
    [Serializable]
    public class Money : PlayerDataBase
    {
        public string Name;
        public int Id;
        public int Amount;
        public const string path = "MoneyData";

    }

    [SerializeField] public static List<Money> Moneys;
    #endregion

    public abstract class PlayerDataBase
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool HasSkill { get; set; }
        public int Amount { get; set; }
        public string path { get; }

        public static List<T> Load<T>(List<T> Data, string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>($"Data/ClientData/{path}");
            Data = CsvUtility.GetEnumerableFromCsv<T>(textAsset.text).ToList();
            return Data;
            //Debug.Log(Data.Count);
        }

        public static void Save<T>(List<T> Data, string path)
        {
            CsvUtility.SaveCsv(Data, $"Assets/0_Multi/Resources/Data/ClientData/{path}");
        }
    }

    public static void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        Debug.Log(Skills[0].Name);
        Debug.Log(Skills[0].Id);
        Debug.Log(Skills[0].HasSkill);
        Debug.Log(Skills.Count);
        Debug.Log(GetName("시작골드증가", 1, Skills));
        PlayerDataBase.Save(Skills, Skill.path);


    }


    #region Get함수
    static int _row = 0;

    public static int GetRow<T>(string Name, int id, List<T> Data) where T : PlayerDataBase
    {
        for (int i = 0; i <= Data.Count; i++)
        {
            if (Data[i].Name == Name && Data[i].Id == id)
            {
                _row = i;
                return _row;
            }
            
        }
        return 0;
    }

    public static string GetName<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].Name;

    }

    public static int GetId<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].Id;
    }

    public static bool GetHasSkill<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].HasSkill;
    }

    public static int Getamount<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].Amount;
    }

    #endregion


    #region Set함수
    public static void SetName<T>(string name, int id, string value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Name = value;
        PlayerDataBase.Save(Data, path);

    }

    public static void SetId<T>(string name, int id, int value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Id = value;
        PlayerDataBase.Save(Data, path);
    }
     
    public static void SetHasSkill<T>(string name, int id, bool value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].HasSkill = value;
        PlayerDataBase.Save(Data, path);
    }

    public static void SetAmount<T>(string name, int id, int value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Amount = value;
        PlayerDataBase.Save(Data, path);
    }
    #endregion
}
