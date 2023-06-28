using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookWorldChangedButton : UI_Scene
{
    [SerializeField] Sprite lookMyWorldIcon;
    [SerializeField] Sprite lookEnemyWorldIcon;

    Button button;
    Text _text;
    protected override void Init()
    {
        base.Init();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(ChangeLookWorld);
        _text = GetComponentInChildren<Text>();
    }

    void ChangeLookWorld()
    {
        Managers.Camera.LookWorldChanged();
        Managers.Sound.PlayEffect(EffectSoundType.PopSound);
        if (Managers.Camera.LookWorld_Id == PlayerIdManager.Id)
        {
            button.image.sprite = lookMyWorldIcon;
            _text.text = "상대 진영으로";
        }
        else
        {
            button.image.sprite = lookEnemyWorldIcon;
            _text.text = "아군 진영으로";
        }
    }
}
