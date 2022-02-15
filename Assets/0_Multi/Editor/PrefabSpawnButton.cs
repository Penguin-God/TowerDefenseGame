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
        //Debug.Log(_prefabs.Length);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("유닛 소환");
        if (GUILayout.Button("에디터에 모든 유닛 소환")) _spawner.AllUnitSpawn_ByEditor();
        EditorGUILayout.Space(10);
        for (int i = 0; i < _prefabs.Length; i++)
        {
            if (_prefabs[i].GetComponentInChildren<TeamSoldier>() == null) continue;

            string _buttonName = _prefabs[i].name + " Spawn";
            _buttonName = _buttonName.Replace('1', ' ');
            if (GUILayout.Button(_buttonName)) _spawner.SpawnUnit(_prefabs[i].name);
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("일반 몬스터 소환");
        int _enemyNumber = 0;
        for (int i = 0; i < _prefabs.Length; i++)
        {
            if (_prefabs[i].GetComponent<Multi_NormalEnemy>() == null) continue;

            string _buttonName = _prefabs[i].name + " Spawn";
            _buttonName = _buttonName.Replace('1', ' ');
            if (GUILayout.Button(_buttonName)) _spawner.SpawnNormalEnemy(_prefabs[i].name, _enemyNumber);
            _enemyNumber++;
        }
    }
}
