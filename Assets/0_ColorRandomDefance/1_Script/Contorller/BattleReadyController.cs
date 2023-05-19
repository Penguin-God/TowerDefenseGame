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
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "��ȯ�� ���͸� ������ �ֽʽÿ�";
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
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "������ ������ �غ� �Ǿ��ٸ� �� ��ư�� Ŭ�����ּ���";
        _readyButton.enabled = true;
        _readyButton.onClick.AddListener(Ready);
    }

    void Ready()
    {
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().text = "�غ� �Ϸ�";
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
