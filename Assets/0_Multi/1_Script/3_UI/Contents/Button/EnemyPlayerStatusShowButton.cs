using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerStatusShowButton : Multi_UI_Scene
{
    bool isShow;
    protected override void Init()
    {
        base.Init();
        GetComponentInChildren<Button>().onClick.AddListener(ShowStatusWinodw);
    }

    void ShowStatusWinodw()
    {
        if (isShow)
            Multi_Managers.UI.ClosePopupUI("EnemyPlayerInfoWindow");
        else
            Multi_Managers.UI.ShowPopupUI<EnemyPlayerInfoWindow>("EnemyPlayerInfoWindow").UpdateCount();
        isShow = !isShow;
    }
}
