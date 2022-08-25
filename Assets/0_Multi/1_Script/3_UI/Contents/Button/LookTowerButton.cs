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
        Multi_Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Multi_Managers.Camera.IsLookEnemyTower)
            Multi_Managers.Camera.LookWorld();
        else
            Multi_Managers.Camera.LookEnemyTower();
    }
}
