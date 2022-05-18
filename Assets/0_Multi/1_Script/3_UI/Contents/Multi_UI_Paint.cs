using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Multi_UI_Paint : Multi_UI_Scene
{
    [SerializeField] GameObject _paintActiveButton;
    protected override void Init()
    {
        base.Init();
        BindEvnet(_paintActiveButton, ChangePaintRootActive);
    }

    [SerializeField] GameObject _paintRoot;
    void ChangePaintRootActive(PointerEventData data) => _paintRoot.SetActive(!_paintRoot.activeSelf);
}
