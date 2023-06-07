using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialScene : BaseScene
{
    protected override void Init()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRandomRoom();

        MultiServiceMidiator.Instance.Init();
        Managers.Unit.Init(new UnitControllerAttacher().AttacherUnitController(MultiServiceMidiator.Instance.gameObject), Managers.Data);
        new WorldInitializer(gameObject).Init();

        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").gameObject.SetActive(false);
        gameObject.AddComponent<Tutorial_AI>();
        Multi_GameManager.Instance.CreateOtherPlayerData(SkillType.검은유닛강화, SkillType.판매보상증가);
        // Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += CheckGameOver;
    }

    void CheckGameOver(int enemyCount)
    {
        if (enemyCount >= 50)
            gameObject.GetComponent<WinOrLossController>().GameEnd("승리!!");
    }
}
