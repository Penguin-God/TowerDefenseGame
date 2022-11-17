using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Barrier : Multi_UI_Base
{
    protected override void Init()
    {
        base.Init();
        BindEvnet(gameObject, (data) => Multi_Managers.UI.ClosePopupUI());
    }
}
