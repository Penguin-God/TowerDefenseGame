using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Multi_ClientData
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

    [SerializeField] public List<Skill> Skills;
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

    [SerializeField] public List<Money> Moneys;
    #endregion

    public abstract class PlayerDataBase
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool HasSkill { get; set; }
        public int Amount { get; set; }
        public string path { get; }

        
    }

    public List<T> Load<T>(List<T> Data, string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/ClientData/{path}");
        Data = CsvUtility.GetEnumerableFromCsv<T>(textAsset.text).ToList();
        return Data;
        //Debug.Log(Data.Count);
    }

    public void Save<T>(List<T> Data, string path)
    {
        CsvUtility.SaveCsv(Data, $"Assets/0_Multi/Resources/Data/ClientData/{path}");
    }

    public void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        Debug.Log(Skills[0].Name);
        Debug.Log(Skills[0].Id);
        Debug.Log(Skills[0].HasSkill);
        Debug.Log(Skills.Count);
        Debug.Log(GetName("시작골드증가", 1, Skills));
        Save(Skills, Skill.path);


    }


    #region Get함수
    static int _row = 0;

    public int GetRow<T>(string Name, int id, List<T> Data) where T : PlayerDataBase
    {
        for (int i = 1; i < Data.Count; i++)
        {
            Debug.Log($"Data : {Data[i].Name} : {Name}");
            Debug.Log($"Dtta : {Data[i].Id} : {id}");
            if (Data[i].Name == Name && Data[i].Id == id)
            {
                Debug.Log("If 문 안에 들어옴");
                _row = i;
                return _row;
            }
            
        }
        return 0;
    }

    public int GetRow(string Name, int id, List<Money> Data)
    {
        for (int i = 1; i < Data.Count; i++)
        {
            Debug.Log($"Data : {Data[i].Name} : {Name}");
            Debug.Log($"Dtta : {Data[i].Id} : {id}");
            if (Data[i].Name == Name && Data[i].Id == id)
            {
                Debug.Log("If 문 안에 들어옴");
                _row = i;
                return _row;
            }

        }
        return 0;
    }

    public string GetName<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].Name;

    }

    public int GetId<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].Id;
    }

    public bool GetHasSkill<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        return Data[row].HasSkill;
    }

    //public int Getamount<T>(string name, int id, List<T> Data) where T : PlayerDataBase
    //{
    //    //Debug.Log(Data.GetType().ToString());
    //    int row = GetRow(name, id, Data);
        
    //    return Data[row].Amount;
    //}

    public int Getamount(string name, int id, List<Money> Data)
    {
        int row = GetRow(name, id, Data);

        return Data[row].Amount;
    }

    #endregion


    #region Set함수
    public void SetName<T>(string name, int id, string value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Name = value;
        Save(Data, path);

    }

    public void SetId<T>(string name, int id, int value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Id = value;
        Save(Data, path);
    }
     
    public void SetHasSkill<T>(string name, int id, bool value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].HasSkill = value;
        Save(Data, path);
    }

    public void SetAmount<T>(string name, int id, int value, string path, List<T> Data) where T : PlayerDataBase
    {
        int row = GetRow(name, id, Data);
        Data[row].Amount = value;
        Save(Data, path);
    }
    #endregion
}
