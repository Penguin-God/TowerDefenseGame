using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCable : MonoBehaviourPun
{
    [SerializeField] int _usingId = -1;
    public int UsingId => _usingId;

    public void SetId_RPC(int id) => gameObject.GetComponent<PhotonView>().RPC("SetId", RpcTarget.All, id);
    [PunRPC] void SetId(int id) => _usingId = id;

    // 위치
    public void SetPosition_RPC(Vector3 _pos) => photonView.RPC("SetPosition", RpcTarget.All, _pos);
    [PunRPC] void SetPosition(Vector3 _pos) => transform.position = _pos;

    // 방향(쿼터니언)
    public void SetRotation_RPC(Quaternion _rot) => photonView.RPC("SetRotation", RpcTarget.All, _rot);
    [PunRPC] void SetRotation(Quaternion _rot) => transform.rotation = _rot;

    // 방향(백터)
    public void SetRotate_RPC(Vector3 _looDir) => photonView.RPC("SetRotate", RpcTarget.All, _looDir);
    [PunRPC] void SetRotate(Vector3 _looDir) => transform.Rotate(_looDir);

    // 활성상태
    public void SetActive_RPC(bool _isActive) => photonView.RPC("SetActive", RpcTarget.All, _isActive);
    [PunRPC] void SetActive(bool _isActive) => gameObject.SetActive(_isActive);

    // 속도
    public void SetVelocity_RPC(Vector3 _velo) => photonView.RPC("SetVelocity", RpcTarget.All, _velo);
    [PunRPC]
    void SetVelocity(Vector3 _velo)
    {
        if (GetComponent<Rigidbody>() == null) return;

        GetComponent<Rigidbody>().velocity = _velo;
    }

    // 파티클
    public void PlayParticle_RPC() => photonView.RPC("PlayParticle", RpcTarget.All);
    [PunRPC] void PlayParticle() => GetComponent<ParticleSystem>()?.Play();
}
