using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EX
{
    #region Skill 구조체
    [Serializable]
    class Skill
    {
        public string skillName;
        public int id;
        public bool hasSkill;

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
    class Money
    {
        public string name;
        public int id;
        public bool amount;

        public static void Load()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/MoneyData");
            Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();
            Debug.Log("실행");
        }

        public static void Save()
        {
            CsvUtility.SaveCsv(Skills, "Assets/0_Multi/Resources/Data/ClientData/MoneyData");
        }
    }

    [SerializeField] static List<Skill> Moneys;
    #endregion

    public static void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/ClientData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        Debug.Log(Skills[0].skillName);
        Debug.Log(Skills[0].id);
        Debug.Log(Skills[0].hasSkill);
        Debug.Log(Skills.Count);
        Debug.Log(GetName("시작골드증가", 1, () => Skill.Load()));
        Skill.Save();


    }


    #region Get함수
    static int _row = 0;

    public static int GetRow(string skillName, int id)
    {
        for (int i = 0; i <= Skills.Count; i++)
        {
            if (Skills[i].skillName == skillName && Skills[i].id == id)
            {
                _row = i;
                return _row;
            }
            
        }
        return 0;
    }

    public static string GetName(string skillName, int id, Action load)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        return Skills[row].skillName;

    }

    public static int GetId(string skillName, int id, Action load)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        return Skills[row].id;
    }

    public static bool GetHasSkill(string skillName, int id, Action load)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        return Skills[row].hasSkill;
    }

    #endregion


    #region Set함수
    public static void SetName(string skillName, int id, string value, Action load, Action save)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        Skills[row].skillName = value;
        save.Invoke();

    }

    public static void SetId(string skillName, int id, int value, Action load, Action save)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        Skills[row].id = value;
        save.Invoke();
    }

    public static void SetHasSkill(string skillName, int id, bool value, Action load, Action save)
    {
        load.Invoke();
        int row = GetRow(skillName, id);
        Skills[row].hasSkill = value;
        save.Invoke();
    }
    #endregion
}
