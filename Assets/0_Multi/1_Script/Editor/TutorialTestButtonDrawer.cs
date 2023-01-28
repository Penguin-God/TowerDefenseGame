using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TutorialTester))]
public class TutorialTestButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TutorialTester tester = (TutorialTester)target;
        if (GUILayout.Button("기본")) tester.AddTutorial<Tutorial_Basic>();
        if (GUILayout.Button("상대방 진영")) tester.AddTutorial<Tutorial_OtherPlayer>();
        if (GUILayout.Button("적군의 성")) tester.AddTutorial<Tutorial_Tower>();
        if (GUILayout.Button("보스")) tester.AddTutorial<Tutorial_Boss>();
        if (GUILayout.Button("조합")) tester.AddTutorial<Tutorial_Combine>();
    }
}
