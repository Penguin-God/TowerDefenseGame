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
    }
}
