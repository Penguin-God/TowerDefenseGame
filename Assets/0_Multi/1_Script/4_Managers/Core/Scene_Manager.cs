using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public enum SceneTyep
{
    클라이언트,
    New_Scene,
    TestScene,
}

public class Scene_Manager
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();
    public SceneTyep CurrentSceneType = SceneTyep.클라이언트;
    public bool IsBattleScene => CurrentSceneType == SceneTyep.New_Scene || CurrentSceneType == SceneTyep.TestScene;

    public void LoadScene(SceneTyep type)
    {
        Multi_Managers.Clear();
        SceneManager.LoadScene(Enum.GetName(typeof(SceneTyep), type));
        CurrentSceneType = type;
    }

    public void LoadLevel(SceneTyep type)
    {
        Multi_Managers.Clear();
        PhotonNetwork.LoadLevel(Enum.GetName(typeof(SceneTyep), type));
        CurrentSceneType = type;
    }

    public void Clear() => CurrentScene?.Clear();
}
