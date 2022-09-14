using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BounceEnergyball : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;

    float originSpeed;
    Vector3 lastVelocity;
    Rigidbody rigid;
    RPCable rpcable;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rpcable = gameObject.GetOrAddComponent<RPCable>();
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
        originSpeed = speed;
    }

    void OnDisable()
    {
        speed = originSpeed;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        lastVelocity = rigid.velocity;
    }

    Renderer _renderer;
    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient == false || collision.gameObject.tag != "Structures") return;

        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        Multi_Managers.Sound.PlayEffect_If(EffectSoundType.MageBallBonce, () => _renderer.isVisible);

        speed += acceleration;
        rpcable.SetVelocity_RPC(dir * speed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(transform.position);
        else transform.position = (Vector3)stream.ReceiveNext();
    }
}
