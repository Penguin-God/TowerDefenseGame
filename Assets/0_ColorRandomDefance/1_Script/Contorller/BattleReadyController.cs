using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BattleReadyController : MonoBehaviourPun
{
    [SerializeField] Button _readyButton;

    int _readyCount;

    void Awake()
    {
        _readyButton.onClick.AddListener(Ready);        
    }

    void Ready() => photonView.RPC(nameof(AddReadyCount), RpcTarget.MasterClient);

    [PunRPC]
    void AddReadyCount()
    {
        _readyCount++;
        if (AllPlayerIsReady(_readyCount))
            Multi_GameManager.Instance.GameStart();
    }

    bool AllPlayerIsReady(int readyCount) => readyCount >= PlayerIdManager.MaxPlayerCount;
}
