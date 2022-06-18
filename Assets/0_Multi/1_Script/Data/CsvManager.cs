using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

[Serializable]
class Tests
{
    [SerializeField] KeyValuePair<int, string> pair;
    [SerializeField] Dictionary<UnitFlags, int> dict;

    public void printData()
    {
        Debug.Log($"{pair.Key} :: {pair.Value}");

        foreach (var item in dict)
        {
            Debug.Log($"{item.Key.ColorNumber} {item.Key.ClassNumber} :: {item.Value}");
        }
    }
}

[Serializable]
class Skill
{
    [SerializeField] string skillName;
    [SerializeField] int id;
    [SerializeField] bool hasSkill;

    public Skill(string name)
    {
        skillName = name;
    }
}

public class CsvManager : MonoBehaviour
{
    [SerializeField] CombineCondition[] combineConditions;
    
    [ContextMenu("Test")]
    void Test()
    {
        KeyValuePair<int, string> pair = new KeyValuePair<int, string>();
        print(pair.GetType().ToString().GetMiddleString("[", "]"));

        Dictionary<UnitFlags, int> dict = new Dictionary<UnitFlags, int>();
        print(dict.GetType().ToString().GetMiddleString("[", "]"));

        Type aaa = new KeyValuePair<int, string>(1, "22").GetType();
        // pair = aaa.GetConstructor(new[] { typeof(int), typeof(int) });
        print($"{pair.Key}, {pair.Value}");
    }

    [SerializeField] List<Skill> Skills;
    void TypeTest()
    {
        // csv 읽어오기
        TextAsset textAsset = Resources.Load<TextAsset>("Data/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        // 저장 버전 1
        string csvText = CsvUtility.EnumerableToCsv(Skills);
        CsvUtility.SaveCsv(csvText, "SkillData");
        // 저장 버전 2
        CsvUtility.SaveCsv(Skills, "SkillData");
    }
}
