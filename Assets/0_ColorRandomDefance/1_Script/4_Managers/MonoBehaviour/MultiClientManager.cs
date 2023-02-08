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
    public Button _gameMatchingButton;

    private bool _isLobby = true;
    void Start()
    {
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.Disconnect();
        _gameMatchingButton.onClick.AddListener(Connect);
        ConnectionInfoText.text = "매치 상태";
    }

    void Connect()
    {
        _gameMatchingButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            ConnectionInfoText.text = "방에 들어가는 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ConnectionInfoText.text = "접속 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        _gameMatchingButton.interactable = true;

        ConnectionInfoText.text = $"연결 됨. Version : {GameVersion}";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (_isLobby)
        {
            _isLobby = false;
            return;
        }
        if (Application.isPlaying == false || _gameMatchingButton == null) return;

        _gameMatchingButton.interactable = false;

        ConnectionInfoText.text = "연결 실패 재접속 중...";

        PhotonNetwork.ConnectUsingSettings();
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
