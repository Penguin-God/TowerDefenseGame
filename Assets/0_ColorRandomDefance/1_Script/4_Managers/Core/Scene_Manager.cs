using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public enum SceneTyep
{
    Lobby,
    Battle,
    Tutorial,
    Matching,
}

public class Scene_Manager
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();
    public SceneTyep CurrentSceneType = SceneTyep.Lobby;

    public void LoadScene(SceneTyep type)
    {
        Managers.Clear();
        SceneManager.LoadScene(Enum.GetName(typeof(SceneTyep), type));
        CurrentSceneType = type;
    }

    public void LoadLevel(SceneTyep type)
    {
        Managers.Clear();
        PhotonNetwork.LoadLevel(Enum.GetName(typeof(SceneTyep), type));
        CurrentSceneType = type;
    }

    public void Clear() => CurrentScene?.Clear();
}
