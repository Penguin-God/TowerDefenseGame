using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiClientManager : MonoBehaviourPunCallbacks
{
    private string GameVersion = "1";
    public Text ConnectionInfoText;

    //public Text ConnectionInfoText;
    public Button MultiStartButton;
    void Start()
    {
        PhotonNetwork.GameVersion = GameVersion;

        PhotonNetwork.ConnectUsingSettings();

        MultiStartButton.interactable = false;

        ConnectionInfoText.text = "Loading...";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) Connect();
    }

    public override void OnConnectedToMaster()
    {
        MultiStartButton.interactable = true;

        ConnectionInfoText.text = $"연결 됨. Version : {GameVersion}";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        MultiStartButton.interactable = false;

        ConnectionInfoText.text = "연결 실패 재접속 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect() // MultiStartButton OnClick
    {
        MultiStartButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            ConnectionInfoText.text = "방에 들어가는 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ConnectionInfoText.text = "연결 실패 재접속 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ConnectionInfoText.text = "빈 방 없음, 새로운 방 생성 중...";

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        ConnectionInfoText.text = "방 참가 성공";

        PhotonNetwork.LoadLevel("MultiLoading");
    }
}
