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

    EnemySpawnNumManager _manager;
    public void EnterBattle(EnemySpawnNumManager manager)
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "소환할 몬스터를 선택해 주십시오";
        _readyButton.enabled = false;
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(false);
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(true);
        
        _manager = manager; ;
        _manager.OnSpawnMonsterChange += ActiveReadyButton;
    }

    void ActiveReadyButton(int num)
    {
        _manager.OnSpawnMonsterChange -= ActiveReadyButton;
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "게임을 시작할 준비가 되었다면 이 버튼을 클릭해주세요";
        _readyButton.enabled = true;
        _readyButton.onClick.AddListener(Ready);
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
        Multi_GameManager.Instance.BattleData.MaxUnit += 0; // ui event
        foreach (var ui in Managers.UI.SceneUIs)
            ui.gameObject.SetActive(true);
        Managers.UI.GetSceneUI<UI_EnemySelector>().gameObject.SetActive(false);
        Multi_GameManager.Instance.GameStart();
    }
}
