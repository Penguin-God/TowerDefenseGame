using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiDevelopHelper))]
public class ConnectButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MultiDevelopHelper _helper = (MultiDevelopHelper)target;
        if (GUILayout.Button("방 생성 및 입장")) _helper.EditorConnect();
    }
}
