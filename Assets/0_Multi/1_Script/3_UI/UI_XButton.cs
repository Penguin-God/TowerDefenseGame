using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_XButton : Multi_UI_Base
{
    protected override void Init()
    {
        base.Init();
        GetComponent<Button>().onClick.AddListener(() => Multi_Managers.UI.ClosePopupUI());
    }
}
