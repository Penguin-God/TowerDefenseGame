using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Manager
{
    // 위치
    public void RPC_Position(PhotonView PV, Vector3 pos) => PV.RPC("SetPosition", RpcTarget.All, PV.ViewID, pos);
    [PunRPC] 
    private void SetPosition(int id, Vector3 pos)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.position = pos;
    }

    // 방향(쿼터니언) 대입
    public void RPC_Rotation(PhotonView PV, Quaternion rot) => PV.RPC("SetRotation", RpcTarget.All, PV.ViewID, rot);
    [PunRPC] 
    private void SetRotation(int id, Quaternion rot)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.rotation = rot;
    }

    // 방향(백터) 회전
    public void RPC_Rotate(PhotonView PV, Vector3 lookDir) => PV.RPC("SetRotate", RpcTarget.All, PV.ViewID, lookDir);
    [PunRPC] private void SetRotate(int id, Vector3 lookDir)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.Rotate(lookDir);
    }

    // 활성상태
    public void RPC_Active(PhotonView PV, bool isActive) => PV.RPC("SetActive", RpcTarget.All, PV.ViewID, isActive);
    [PunRPC] private void SetActive(int id, bool isActive)
    {
        GameObject target = GetGameObjectFromViewID(id);
        if (target != null) target.SetActive(isActive);
    }

    // 속도
    public void RPC_Velocity(PhotonView PV, Vector3 velo) => PV.RPC("SetVelocity", RpcTarget.All, PV.ViewID, velo);
    [PunRPC]
    private void SetVelocity(int id, Vector3 velo)
    {
        Rigidbody target = GetRigidbodyFromViewID(id);
        if(target != null) target.velocity = velo;
    }

    Transform GetTransformFromViewID(int id) => PhotonView.Find(id).transform;
    GameObject GetGameObjectFromViewID(int id) => PhotonView.Find(id).gameObject;
    Rigidbody GetRigidbodyFromViewID(int id) => PhotonView.Find(id).GetComponent<Rigidbody>();
}
