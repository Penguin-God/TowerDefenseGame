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

    public void ClickMultiStartButton()
    {
        if (!photonView.IsMine)
        {
            GameStartCount += 1;
            return;          
        }

        
        MultiGameStartButton.interactable = false;
        if (GameStartCount == 2)
        {
            PhotonNetwork.LoadLevel("합친 씬 - 장익준 멀티");
        }
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
        PhotonNetwork.LoadLevel("박준 멀티");
        //PhotonNetwork.LoadLevel("합친 씬 - 장익준 멀티");


    }
}
