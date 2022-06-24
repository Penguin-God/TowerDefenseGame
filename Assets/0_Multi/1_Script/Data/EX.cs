using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EX
{
    #region Skill 구조체
    [Serializable]
    public class Skill : I_M
    {
        public string Name;
        public int Id;
        public bool HasSkill;

        public static void Load()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/SkillData");
            Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();
            Debug.Log("실행");
        }

        public static void Save()
        {
            CsvUtility.SaveCsv(Skills, "Assets/0_Multi/Resources/Data/ClientData/SkillData");
        }
    }

    [SerializeField] static List<Skill> Skills;
    #endregion

    #region Money 구조체
    [Serializable]
    public class Money : I_M
    {
        public string Name;
        public int Id;
        public int Amount;

        public static void Load()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/MoneyData");
            Moneys = CsvUtility.GetEnumerableFromCsv<Money>(textAsset.text).ToList();
            Debug.Log("실행");
        }

        public static void Save()
        {
            CsvUtility.SaveCsv(Moneys, "Assets/0_Multi/Resources/Data/ClientData/MoneyData");
        }
    }

    [SerializeField] static List<Money> Moneys;
    #endregion

    public class I_M
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool HasSkill { get; set; }
        public int Amount { get; set; }

    }

    public static void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        Debug.Log(Skills[0].Name);
        Debug.Log(Skills[0].Id);
        Debug.Log(Skills[0].HasSkill);
        Debug.Log(Skills.Count);
        Debug.Log(GetName("시작골드증가", 1, () => Skill.Load(), Skills));
        Skill.Save();


    }


    #region Get함수
    static int _row = 0;

    public static int GetRow<T>(string Name, int id, List<T> Data) where T : I_M
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

    public static string GetName<T>(string name, int id, Action load, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        return Data[row].Name;

    }

    public static int GetId<T>(string name, int id, Action load, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        return Data[row].Id;
    }

    public static bool GetHasSkill<T>(string name, int id, Action load, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        return Data[row].HasSkill;
    }

    public static int Getamount<T>(string name, int id, Action load, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        return Data[row].Amount;
    }

    #endregion


    #region Set함수
    public static void SetName<T>(string name, int id, string value, Action load, Action save, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        Data[row].Name = value;
        save.Invoke();

    }

    public static void SetId<T>(string name, int id, int value, Action load, Action save, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        Data[row].Id = value;
        save.Invoke();
    }
     
    public static void SetHasSkill<T>(string name, int id, bool value, Action load, Action save, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        Data[row].HasSkill = value;
        save.Invoke();
    }

    public static void SetAmount<T>(string name, int id, int value, Action load, Action save, List<T> Data) where T : I_M
    {
        load.Invoke();
        int row = GetRow(name, id, Data);
        Data[row].Amount = value;
        save.Invoke();
    }
    #endregion
}
