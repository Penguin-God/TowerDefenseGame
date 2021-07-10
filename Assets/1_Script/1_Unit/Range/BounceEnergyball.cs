using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEnergyball : MonoBehaviour
{
    Vector3 lastVelocity;
    Rigidbody rigid;
    private void Awake()
    {
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
        float speed = lastVelocity.magnitude;
        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        //transform.rotation = Quaternion.Euler(new Vector3(0, dir.y, 0));

        rigid.velocity = dir * 100;
    }
}
