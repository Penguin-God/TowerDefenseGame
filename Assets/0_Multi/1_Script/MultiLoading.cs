using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiLoading : MonoBehaviourPun
{
    [SerializeField] Text stateText = null;
    
    void Update()
    {
        stateText.text = PhotonNetwork.PlayerList.Length + "";
        if (Input.GetKeyDown(KeyCode.E)) EnterBattle();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            EnterBattle();
            photonView.RPC("EnterBattle", RpcTarget.MasterClient);
        }
    }

    [PunRPC, ContextMenu("InitBattleScene")]
    void EnterBattle() => Multi_Managers.Scene.LoadLevel(SceneTyep.New_Scene);
}
