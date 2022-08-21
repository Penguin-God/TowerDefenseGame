using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiDevelopHelper))]
public class MultiDevelopButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MultiDevelopHelper _helper = (MultiDevelopHelper)target;
        if (GUILayout.Button("방 생성 및 입장")) _helper.EditorConnect();
        if (GUILayout.Button("씬 카메라 호스트 월드로 이동")) SetSceneViewCamera(true);
        if (GUILayout.Button("씬 카메라 클라이언트 월드로 이동")) SetSceneViewCamera(false);
    }


    void SetSceneViewCamera(bool _isLookHost)
    {
        var sceneCamera = SceneView.lastActiveSceneView;
        sceneCamera.LookAt(_isLookHost ? new Vector3(0, 0, 0) : new Vector3(0, 0, 500));
    }
}
