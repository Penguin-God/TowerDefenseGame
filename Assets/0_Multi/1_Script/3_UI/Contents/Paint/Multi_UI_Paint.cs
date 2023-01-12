using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Multi_UI_Paint : UI_Scene
{
    [SerializeField] GameObject _currentUnitTracker;
    public GameObject CurrentUnitTracker { get => _currentUnitTracker; set => _currentUnitTracker = value; }

    [SerializeField] GameObject _paintActiveButton;
    protected override void Init()
    {
        base.Init();

        _paintActiveButton.GetComponent<Button>().onClick.AddListener(ChangePaintRootActive);
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive()
    {
        _paintRoot.SetActive(!_paintRoot.activeSelf);
        Managers.Sound.PlayEffect(EffectSoundType.PopSound_2);
    }
}
