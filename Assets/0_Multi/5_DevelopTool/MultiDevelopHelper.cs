using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiDevelopHelper : MonoBehaviourPunCallbacks
{
    [SerializeField] Text guideText = null;


    public void EditorConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        guideText.text = $"서버 연결 됨.";
        Connect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        guideText.text = "연결 실패 재접속 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect() 
    {
        if (PhotonNetwork.IsConnected)
        {
            guideText.text = "방에 들어가는 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            guideText.text = "연결 실패 재접속 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinedRoom()
    {
        guideText.text = "방 참가 성공";

        PhotonNetwork.LoadLevel("박준 멀티");
    }

    // 방 접속 실패 시 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        guideText.text = "빈 방 없음, 새로운 방 생성 중...";

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
}
