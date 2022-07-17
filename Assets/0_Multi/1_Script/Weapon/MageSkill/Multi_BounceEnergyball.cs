using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BounceEnergyball : MonoBehaviourPun, IPunObservable
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;

    float originSpeed;
    AudioSource audioSource;
    Vector3 lastVelocity;
    Rigidbody rigid;
    RPCable rpcable;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        rpcable = gameObject.GetOrAddComponent<RPCable>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient == false || collision.gameObject.tag != "Structures") return;

        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        //audioSource.Play();

        speed += acceleration;
        rpcable.SetVelocity_RPC(dir * speed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(transform.position);
        else transform.position = (Vector3)stream.ReceiveNext();
    }
}
