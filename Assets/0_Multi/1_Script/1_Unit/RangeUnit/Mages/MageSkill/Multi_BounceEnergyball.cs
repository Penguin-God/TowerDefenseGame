using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BounceEnergyball : MonoBehaviourPun
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    float originSpeed;
    AudioSource audioSource;

    Vector3 lastVelocity;
    Rigidbody rigid;
    MyPunRPC myPunRPC;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        myPunRPC = GetComponent<MyPunRPC>();
        originSpeed = speed;
    }

    void OnEnable()
    {
        speed = originSpeed;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        lastVelocity = rigid.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (collision.gameObject.tag != "Structures")
        {
            Debug.LogError("이건 아니야");
            return;
        }

        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        audioSource.Play();

        Multi_Managers.RPC.RPC_Rotate(photonView.ViewID, dir);
        //myPunRPC.RPC_Rotate(dir);
        speed += acceleration;
        Multi_Managers.RPC.RPC_Velocity(photonView.ViewID, dir * speed);
        //myPunRPC.RPC_Velocity(dir * speed);
    }
}
