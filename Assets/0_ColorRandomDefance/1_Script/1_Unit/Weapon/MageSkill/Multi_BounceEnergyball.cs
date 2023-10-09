using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BounceEnergyball : MonoBehaviour // , IPunObservable
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;

    float currentSpeed;
    Vector3 lastVelocity;
    Rigidbody rigid;
    // RPCable rpcable;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        // rpcable = gameObject.GetOrAddComponent<RPCable>();
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
        currentSpeed = speed;
    }

    void OnDisable()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        // if (PhotonNetwork.IsMasterClient == false) return;
        lastVelocity = rigid.velocity;
    }

    Renderer _renderer;
    private void OnCollisionEnter(Collision collision)
    {
        // if (PhotonNetwork.IsMasterClient == false || collision.gameObject.tag != "Structures") return;
        if (collision.gameObject.tag != "Structures") return;

        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal).normalized;
        if(_renderer.isVisible)
            Managers.Sound.PlayEffect(EffectSoundType.MageBallBonce);

        currentSpeed += acceleration;
        rigid.velocity = dir * currentSpeed;
        // rpcable.SetVelocity_RPC(dir * currentSpeed);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting) stream.SendNext(transform.position);
    //    else transform.position = (Vector3)stream.ReceiveNext();
    //}
}
