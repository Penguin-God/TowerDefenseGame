using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Barrier : UI_Base
{
    protected override void Init()
    {
        base.Init();
        BindEvnet(gameObject, (data) => Managers.UI.ClosePopupUI());
    }
}
