﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        Multi_Managers.Skill.Init();
        Multi_Managers.Camera.Init();
        Init_UI();

        if (PhotonNetwork.IsMasterClient == false) return;
        Multi_Managers.Pool.Init();
    }

    void Init_UI()
    {
        Multi_Managers.UI.Init();

        Multi_Managers.UI.ShowPopupUI<BackGround>("BackGround");
        Multi_Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");
        Multi_Managers.UI.ShowPopupUI<WarningText>();
        Multi_Managers.UI.ShowPopupUI<RandomShop_UI>("InGameShop/Random Shop");
        Multi_Managers.UI.ShowPopupUI<EnemyPlayerInfoWindow>("EnemyPlayerInfoWindow");

        Multi_Managers.UI.ShowSceneUI<LookWorldChangedButton>("LookWorldChangedButton");
        Multi_Managers.UI.ShowSceneUI<EnemySelector_UI>("EnemySelector_UI");
        Multi_Managers.UI.ShowSceneUI<Multi_UI_Paint>("Paint");
        Multi_Managers.UI.ShowSceneUI<UnitCountTracker>("CurrentUnit Panel");
        Multi_Managers.UI.ShowSceneUI<CreateDefenserButton>("Create Defenser Button");
        Multi_Managers.UI.ShowSceneUI<LookTowerButton>("Story Wolrd Enter Button");
        Multi_Managers.UI.ShowSceneUI<EnemyPlayerStatusShowButton>("EnemyPlayerStatusShowButton");
    }

    public override void Clear()
    {
        Multi_Managers.Pool.Clear();
    }
}