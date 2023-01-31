using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(UserSkillTest))]
public class UserSkillTesterDrawer : Editor
{
    UserSkillTest userSkillTest;
    private void OnEnable()
    {
        userSkillTest = (UserSkillTest)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawButton();
    }

    // TODO : 비활성화도 구현하기
    void DrawButton()
    {
        foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
        {
            if (GUILayout.Button(skillType.ToString()))
                userSkillTest.ActiveSkill(skillType);
        }
    }
}
