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
        Managers.UI.ShowPopupUI<UI_UnitManagedWindow>("UnitManagedWindow");
    }
}
