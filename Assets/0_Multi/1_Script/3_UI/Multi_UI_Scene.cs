using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_UI_Scene : Multi_UI_Base
{
    protected override void Init()
    {
        Multi_Managers.UI.SetCanvas(gameObject, false);
    }
}
