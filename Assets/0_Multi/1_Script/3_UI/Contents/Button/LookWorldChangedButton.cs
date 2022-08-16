using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookWorldChangedButton : Multi_UI_Base
{
    protected override void Init()
    {
        GetComponent<Button>().onClick.AddListener(Multi_Managers.Camera.LookWorldChanged);
    }
}
