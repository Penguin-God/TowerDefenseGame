using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameObject[] _prefabs = Resources.LoadAll<GameObject>("");
        PrefabSpawner _spawner = (PrefabSpawner)target;

        DrawUnitSpawnButton(_spawner.allUnit, _spawner);
        DrawEnemySpawnButton(_prefabs, _spawner);
        DrawClineWorldSpawn(_spawner);
    }

    bool showButton = true;
    bool showButton2 = true;
    void DrawUnitSpawnButton(DrawButtonUnits[] _units, PrefabSpawner _spawner)
    {
        EditorGUILayout.Space(20);
        showButton = EditorGUILayout.Foldout(showButton, "에디터에서 유닛 소환");
        if (showButton)
        {
            for (int i = 0; i < _units.Length; i++)
            {
                for (int j = 0; j < _units[i].units.Length; j++)
                {
                    string _buttonName = _units[i].units[j].name + " Spawn";
                    _buttonName = _buttonName.Replace('1', ' ');
                    if (GUILayout.Button(_buttonName)) _spawner.SpawnUnit(i, j);
                }
                EditorGUILayout.Space(5);
            }
        }

        EditorGUILayout.Space(10);
        showButton2 = EditorGUILayout.Foldout(showButton2, "에디터 외 기기에서 유닛 소환");
        if (showButton2)
        {
            for (int i = 0; i < _units.Length; i++)
            {
                for (int j = 0; j < _units[i].units.Length; j++)
                {
                    string _buttonName = _units[i].units[j].name + " Spawn";
                    _buttonName = _buttonName.Replace('1', ' ');
                    if (GUILayout.Button(_buttonName)) _spawner.SpawnUnit_ByClient(i, j);
                }
                EditorGUILayout.Space(5);
            }
        }
    }

    void DrawEnemySpawnButton(GameObject[] _prefabs, PrefabSpawner _spawner)
    {
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("일반 몬스터 소환");
        int _enemyNumber = 0;
        for (int i = 0; i < _prefabs.Length; i++)
        {
            if (_prefabs[i].GetComponent<Multi_NormalEnemy>() == null) continue;

            string _buttonName = _prefabs[i].name + " Spawn";
            _buttonName = _buttonName.Replace('1', ' ');
            if (GUILayout.Button(_buttonName)) _spawner.SpawnNormalEnemy(_enemyNumber);
            _enemyNumber++;
        }
    }

    int colorNum;
    int classNum;
    void DrawClineWorldSpawn(PrefabSpawner _spawner)
    {
        EditorGUILayout.Space(20);
        colorNum = EditorGUILayout.IntField(colorNum);
        classNum = EditorGUILayout.IntField(classNum);
        if (GUILayout.Button("상대 진영에 유닛 소환")) _spawner.SpawnUnit_ByClientWolrd(colorNum, classNum);
    }
}
