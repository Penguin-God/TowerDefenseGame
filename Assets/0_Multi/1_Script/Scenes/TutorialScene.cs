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
        InitSpawnEvnet();
    }

    void InitSpawnEvnet()
    {
        Multi_GameManager.instance.OnStart += () => Multi_SpawnManagers.TowerEnemy.Spawn(1);
        Multi_StageManager.Instance.OnUpdateStage += SpawnBoss;

        void SpawnBoss(int stage)
        {
            if (stage % 10 != 0) return;
            Multi_SpawnManagers.BossEnemy.Spawn(1);
        }
    }
}
