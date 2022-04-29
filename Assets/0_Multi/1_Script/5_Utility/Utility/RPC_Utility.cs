using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPC_Utility : MonoBehaviourPun
{
    private static RPC_Utility instance;
    public static RPC_Utility Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RPC_Utility>();
                if (instance == null)
                {
                    instance = new GameObject("RPC_Utility").AddComponent<RPC_Utility>();
                    instance.gameObject.AddComponent<PhotonView>();
                }
            }

            return instance;
        }
    }

    // 위치
    public void RPC_Position(int viewID, Vector3 pos) => photonView.RPC("SetPosition", RpcTarget.All, viewID, pos);
    [PunRPC] 
    private void SetPosition(int id, Vector3 pos)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.position = pos;
    }

    // 방향(쿼터니언) 대입
    public void RPC_Rotation(int viewID, Quaternion rot) => photonView.RPC("SetRotation", RpcTarget.All, viewID, rot);
    [PunRPC] 
    private void SetRotation(int id, Quaternion rot)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.rotation = rot;
    }

    // 방향(백터) 회전
    public void RPC_Rotate(int viewID, Vector3 lookDir) => photonView.RPC("SetRotate", RpcTarget.All, viewID, lookDir);
    [PunRPC] private void SetRotate(int id, Vector3 lookDir)
    {
        Transform target = GetTransformFromViewID(id);
        if (target != null) target.Rotate(lookDir);
    }

    // 활성상태
    public void RPC_Active(int viewID, bool isActive) => photonView.RPC("SetActive", RpcTarget.All, viewID, isActive);
    [PunRPC] private void SetActive(int id, bool isActive)
    {
        GameObject target = GetGameObjectFromViewID(id);
        if (target != null) target.SetActive(isActive);
    }

    // 속도
    public void RPC_Velocity(int viewID, Vector3 velo) => photonView.RPC("SetVelocity", RpcTarget.All, viewID, velo);
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
