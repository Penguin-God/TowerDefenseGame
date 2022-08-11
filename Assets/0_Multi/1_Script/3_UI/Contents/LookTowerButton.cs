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
        Multi_Managers.UI.CloseAllPopupUI();
        if (Multi_Managers.Camera.IsLookEnemyTower)
            Multi_Managers.Camera.LookWorld();
        else
            Multi_Managers.Camera.LookEnemyTower();
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
