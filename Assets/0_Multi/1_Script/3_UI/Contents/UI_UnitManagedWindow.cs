using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitManagedWindow : Multi_UI_Scene
{
    protected override void Init()
    {
        base.Init();
    }

    [SerializeField] Text _description;
    [SerializeField] Text _currentWolrd;
    public void Show(UI_UnitWindowData data)
    {
        gameObject.SetActive(true);

        _description.text = data.Description;
    }
}
