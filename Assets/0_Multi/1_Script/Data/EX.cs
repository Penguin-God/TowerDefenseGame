using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EX : MonoBehaviour
{
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

    [SerializeField] List<Skill> Skills;
    [ContextMenu("save")]
    public void Testf()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Data/SkillData/SkillData");
        Skills = CsvUtility.GetEnumerableFromCsv<Skill>(textAsset.text).ToList();

        // Debug.Log(Skills[0]);
    }
}
