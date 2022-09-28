using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookTowerButton : Multi_UI_Scene
{
    Text _text;
    void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(CameraPositionChanged);
        _text = GetComponentInChildren<Text>();
    }

    void CameraPositionChanged()
    {
        Multi_Managers.UI.CloseAllPopupUI();
        Multi_Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Multi_Managers.Camera.IsLookEnemyTower)
        {
            Multi_Managers.Camera.LookWorld();
            _text.text = "적군의 성으로";
        }
        else
        {
            Multi_Managers.Camera.LookEnemyTower();
            _text.text = "월드로";
        }
    }
}
