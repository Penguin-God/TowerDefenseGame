using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TutorialScene : BaseScene
{
    [SerializeField] GameObject container;
    protected override void Init()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRandomRoom();
        new WorldInitializer(container).Init();
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow").gameObject.SetActive(false);
        gameObject.AddComponent<Tutorial_AI>();
        Multi_GameManager.instance.CreateOtherPlayerData(SkillType.검은유닛강화, SkillType.판매보상증가);
        Multi_EnemyManager.Instance.OnOtherEnemyCountChanged += CheckGameOver;
    }

    void CheckGameOver(int enemyCount)
    {
        if (enemyCount >= 50)
            container.GetComponent<WinOrLossController>().GameEnd("승리!!");
    }
}
