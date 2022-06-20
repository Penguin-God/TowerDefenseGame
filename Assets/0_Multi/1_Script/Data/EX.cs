using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EX
{
    [Serializable]
    class Skill
    {
        public string skillName;
        public int id;
        public bool hasSkill;
    }

    [SerializeField] static List<Skill> Skills;

    [ContextMenu("save")]
    public static void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/SkillData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        Debug.Log(Skills[0].skillName);
        Skills[0].skillName = "바꾸기";
        Debug.Log(Skills[0].skillName);
        Debug.Log(Skills[0].id);
        Debug.Log(Skills[0].hasSkill);
        Debug.Log(Skills.Count);

        Save();


    }

    public static void Save()
    {
        CsvUtility.SaveCsv(Skills, "Assets/0_Multi/Resources/Data/SkillData/SkillData");
    }

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

    public static string GetName(string skillName, int id)
    {
        int row = GetRow(skillName, id);
        return Skills[row].skillName;

    }

    public static int GetId(string skillName, int id)
    {
        int row = GetRow(skillName, id);
        return Skills[row].id;
    }

    public static bool GetHasSkill(string skillName, int id)
    {
        int row = GetRow(skillName, id);
        return Skills[row].hasSkill;
    }

    public static void SetName(string skillName, int id, string value)
    {
        int row = GetRow(skillName, id);
        Skills[row].skillName = value;

    }

    public static void SetId(string skillName, int id, int value)
    {
        int row = GetRow(skillName, id);
        Skills[row].id = value;
    }

    public static void SetHasSkill(string skillName, int id, bool value)
    {
        int row = GetRow(skillName, id);
        Skills[row].hasSkill = value;
    }
}
