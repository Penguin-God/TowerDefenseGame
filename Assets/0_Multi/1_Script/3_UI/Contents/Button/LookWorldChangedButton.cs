using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookWorldChangedButton : Multi_UI_Scene
{
    [SerializeField] Sprite lookMyWorldIcon;
    [SerializeField] Sprite lookEnemyWorldIcon;

    Button button;
    protected override void Init()
    {
        base.Init();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(ChangeLookWorld);
    }

    void ChangeLookWorld()
    {
        Multi_Managers.Camera.LookWorldChanged();
        Multi_Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Multi_Managers.Camera.LookWorld_Id == Multi_Data.instance.Id)
            button.image.sprite = lookMyWorldIcon;
        else
            button.image.sprite = lookEnemyWorldIcon;
    }
}
