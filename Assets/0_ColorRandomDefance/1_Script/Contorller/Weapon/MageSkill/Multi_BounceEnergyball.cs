using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_BounceEnergyball : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    float currentSpeed;
    Vector3 lastVelocity;
    Rigidbody rigid;
    Renderer _renderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        _renderer = gameObject.GetOrAddComponent<MeshRenderer>();
        currentSpeed = speed;
    }

    void OnDisable()
    {
        currentSpeed = speed;
    }

    void FixedUpdate()
    {
        lastVelocity = rigid.velocity;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Structures") return;

        if (_renderer.isVisible) Managers.Sound.PlayEffect(EffectSoundType.MageBallBonce);

        Vector3 dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal).normalized;
        currentSpeed += acceleration;
        rigid.velocity = dir * currentSpeed;
        transform.position += dir * 0.1f;
    }
}
