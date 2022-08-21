﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiDevelopHelper : MonoBehaviourPunCallbacks
{
    [SerializeField] SceneTyep sceneTyep;
    void Awake() => Screen.SetResolution(960, 540, false);
    public void EditorConnect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => Connect();

    public override void OnDisconnected(DisconnectCause cause) => PhotonNetwork.ConnectUsingSettings();

    public void Connect()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.JoinRandomRoom();
        else PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom() => Multi_Managers.Scene.LoadLevel(sceneTyep);

    // 방 접속 실패 시 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message) => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
}
