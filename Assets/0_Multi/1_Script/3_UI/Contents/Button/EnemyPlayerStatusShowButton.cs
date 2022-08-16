using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerStatusShowButton : Multi_UI_Scene
{
    protected override void Init()
    {
        base.Init();
        GetComponentInChildren<Button>().onClick.AddListener(ShowStatusWinodw);
    }

    void ShowStatusWinodw()
    {
        Multi_Managers.UI.ShowPopupUI<EnemyPlayerInfoWindow>("EnemyPlayerInfoWindow").UpdateCount();
    }
}
