using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEnergyball : MonoBehaviour
{
    AudioSource audioSource;
    Vector3 lastVelocity;
    Rigidbody rigid;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lastVelocity = rigid.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Structures")
        {
            Debug.LogError("이건 아니야");
            return;
        }
        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        audioSource.Play();

        rigid.velocity = dir * 100;
    }
}
