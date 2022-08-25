using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Multi_UI_Paint : Multi_UI_Scene
{
    [SerializeField] GameObject _currentUnitTracker;
    public GameObject CurrentUnitTracker { get => _currentUnitTracker; set => _currentUnitTracker = value; }

    [SerializeField] GameObject _paintActiveButton;
    protected override void Init()
    {
        base.Init();

        BindEvnet(_paintActiveButton, ChangePaintRootActive);
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive(PointerEventData data)
    {
        _paintRoot.SetActive(!_paintRoot.activeSelf);
        Multi_Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }
}
