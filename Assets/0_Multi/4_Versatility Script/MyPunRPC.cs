﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 귀찮아서 만든 여러 상태를 조정하는 class
public class MyPunRPC : MonoBehaviourPun
{
    public void RPC_Position(Vector3 _pos)
    {
        photonView.RPC("SetPosition", RpcTarget.All, _pos);
    }
    [PunRPC] public void SetPosition(Vector3 _pos) => transform.position = _pos;

    public void RPC_Rotation(Quaternion _rot)
    {
        photonView.RPC("SetRotation", RpcTarget.All, _rot);
    }
    [PunRPC] public void SetRotation(Quaternion _rot) => transform.rotation = _rot;

    public void RPC_Rotate(Vector3 _dir)
    {
        photonView.RPC("SetRotate", RpcTarget.All, _dir);
    }
    [PunRPC] public void SetRotate(Vector3 _dir) => transform.Rotate(_dir);

    public void RPC_Active(bool _isActive)
    {
        photonView.RPC("SetActive", RpcTarget.All, _isActive);
    }
    [PunRPC] public void SetActive(bool _isActive) => gameObject.SetActive(_isActive);
}
