using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookTowerButton : Multi_UI_Scene
{
    void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(CameraPositionChanged);
    }

    void CameraPositionChanged()
    {
        if (Multi_GameManager.instance.playerEnterStoryMode)
            LookWorld();
        else
            LookTower();
    }

    void LookWorld()
    {
        Camera.main.transform.position = Multi_Data.instance.CameraPosition;
        Multi_GameManager.instance.playerEnterStoryMode = false;
    }

    void LookTower()
    {
        Camera.main.transform.position = Multi_Data.instance.CameraPosition_LookAtTower;
        Multi_GameManager.instance.playerEnterStoryMode = true;
    }
}
