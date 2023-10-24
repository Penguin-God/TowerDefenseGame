using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PrefabSpawner _spawner = (PrefabSpawner)target;

        DrawUnitSpawnButton(_spawner);
        DrawEnemySpawnButton(_spawner);
    }

    bool showButton = true;
    bool showButton2 = true;

    void DrawUnitSpawnButton(PrefabSpawner _spawner)
    {
        EditorGUILayout.Space(20);
        showButton = EditorGUILayout.Foldout(showButton, "에디터에서 유닛 소환");
        if (showButton)
            CreateUnitSpawnButtons(_spawner.SpawnUnit);

        EditorGUILayout.Space(10);
        showButton2 = EditorGUILayout.Foldout(showButton2, "에디터 외 기기에서 유닛 소환");
        if (showButton2)
            CreateUnitSpawnButtons(_spawner.SpawnUnit_ByClient);
    }

    void CreateUnitSpawnButtons(Action<UnitFlags> spawnUnit)
    {
        foreach (UnitColor color in UnitFlags.AllColors)
        {
            foreach (UnitClass unitClass in UnitFlags.AllClass)
            {
                UnitFlags flag = new UnitFlags(color, unitClass);
                string _buttonName = UnitTextPresenter.GetUnitName(flag) + " 소환";
                if (GUILayout.Button(_buttonName)) spawnUnit(flag);
            }
            EditorGUILayout.Space(5);
        }
    }

    void DrawEnemySpawnButton(PrefabSpawner _spawner)
    {
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("보스 소환");
        const int MaxBossLevel = 5;
        for (int i = 0; i < MaxBossLevel; i++)
        {
            string buttonName = $"레벨 {i + 1} 보스 소환";
            if (GUILayout.Button(buttonName)) _spawner.SpawnBoss(i + 1);
        }
    }
}
