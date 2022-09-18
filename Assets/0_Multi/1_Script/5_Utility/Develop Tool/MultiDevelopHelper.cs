using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiDevelopHelper : MonoBehaviourPunCallbacks
{
    [SerializeField] SceneTyep sceneTyep;
    public void EditorConnect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => Connect();

    public override void OnDisconnected(DisconnectCause cause) => PhotonNetwork.ConnectUsingSettings();

    void Connect()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.JoinRandomRoom();
        else PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Multi_GameManager.instance.AddGold(5000);
            Multi_GameManager.instance.AddFood(1000);
        }

        if (Input.GetKeyDown(KeyCode.E)) EditorConnect();
    }

    public override void OnJoinedRoom() => Multi_Managers.Scene.LoadLevel(sceneTyep);

    // 방 접속 실패 시 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message) => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
}
