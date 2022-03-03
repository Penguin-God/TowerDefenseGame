using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multi_BounceEnergyball : MonoBehaviour
{
    [SerializeField] float speed;
    AudioSource audioSource;
    Vector3 lastVelocity;
    Rigidbody rigid;
    MyPunRPC myPunRPC;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        myPunRPC = GetComponent<MyPunRPC>();
    }

    void OnEnable()
    {
        myPunRPC.RPC_Velocity(transform.forward * speed);
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

        myPunRPC.RPC_Rotate(dir);
        myPunRPC.RPC_Velocity(dir * speed);
    }
}
