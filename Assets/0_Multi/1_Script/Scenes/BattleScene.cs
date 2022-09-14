using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        if (PhotonNetwork.InRoom == false)
        {
            print("방에 없누 ㅋㅋ");
            return;
        }
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        Multi_SpawnManagers.Instance.Init();
        Multi_Managers.Skill.Init();
        Multi_Managers.Camera.Init();
        Init_UI();

        if (PhotonNetwork.IsMasterClient == false) return;
        Multi_Managers.Pool.Init();
    }

    void Start()
    {
        if (PhotonNetwork.InRoom == false) return;

        Multi_Managers.Sound.BattleSceneInit();
    }

    void Init_UI()
    {
        Multi_Managers.UI.Init();

        Multi_Managers.UI.ShowPopupUI<BackGround>("BackGround");
        Multi_Managers.UI.ShowPopupUI<CombineResultText>("CombineResultText");
        Multi_Managers.UI.ShowPopupUI<WarningText>();
        Multi_Managers.UI.ShowPopupUI<RandomShop_UI>("InGameShop/Random Shop");
        Multi_Managers.UI.ShowPopupUI<EnemyPlayerInfoWindow>("EnemyPlayerInfoWindow");

        Multi_Managers.UI.ShowSceneUI<Status_UI>();
        Multi_Managers.UI.ShowSceneUI<LookWorldChangedButton>("LookWorldChangedButton");
        Multi_Managers.UI.ShowSceneUI<EnemySelector_UI>("EnemySelector_UI");
        Multi_Managers.UI.ShowSceneUI<Multi_UI_Paint>("Paint");
        Multi_Managers.UI.ShowSceneUI<CreateDefenserButton>("Create Defenser Button");
        Multi_Managers.UI.ShowSceneUI<LookTowerButton>("Story Wolrd Enter Button");
        Multi_Managers.UI.ShowSceneUI<EnemyPlayerStatusShowButton>("EnemyPlayerStatusShowButton");
    }

    public override void Clear()
    {
        EventIdManager.Clear();
        Multi_Managers.Pool.Clear();
    }
}
