using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum SceneTyep
{
    클라이언트,
    New_Scene,
}

public class Scene_Manager
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    public void LoadScene(SceneTyep type)
    {
        Multi_Managers.Clear();
        SceneManager.LoadScene(Enum.GetName(typeof(SceneTyep), type));
    }

    public void Clear() => CurrentScene.Clear();
}
