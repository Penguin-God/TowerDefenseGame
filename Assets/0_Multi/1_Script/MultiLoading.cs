using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiLoading : MonoBehaviourPun
{
    // 게임 시작 버튼으로 카운트를 올려서 2명이 누르면 게임 시작
    public Button MultiGameStartButton;

    [SerializeField] Text stateText = null;
    int GameStartCount = 0;

    void Update()
    {
        stateText.text = PhotonNetwork.CountOfPlayers.ToString();
    }

    // 이거 사용중
    public void TestClickMultiStartButton()
    {
        //if (!photonView.IsMine)
        //{
        //    GameStartCount += 1;
        //    return;
        //}
        MultiGameStartButton.interactable = false;
        Multi_Managers.Scene.LoadLevel(SceneTyep.New_Scene);
    }
}
