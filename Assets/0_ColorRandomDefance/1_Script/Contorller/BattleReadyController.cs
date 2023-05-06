using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class BattleReadyController : MonoBehaviourPun
{
    [SerializeField] Button _readyButton;

    int _readyCount;

    public void EnterBattle()
    {
        _readyButton.onClick.AddListener(Ready);
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(true);
    }

    void Ready()
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "준비 완료";
        _readyButton.enabled = false;
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);

        photonView.RPC(nameof(AddReadyCount), RpcTarget.MasterClient);
    }

    [PunRPC]
    void AddReadyCount()
    {
        _readyCount++;
        if (AllPlayerIsReady(_readyCount))
            photonView.RPC(nameof(BattleStart), RpcTarget.All);
    }

    bool AllPlayerIsReady(int readyCount) => readyCount >= PhotonNetwork.CurrentRoom.PlayerCount;

    [PunRPC]
    void BattleStart()
    {
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(true);
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);
        Multi_GameManager.Instance.GameStart();
    }
}
