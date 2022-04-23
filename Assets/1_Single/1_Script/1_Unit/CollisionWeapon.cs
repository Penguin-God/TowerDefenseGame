using UnityEngine;
using System;

public class CollisionWeapon : MonoBehaviour
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격

    Rigidbody Rigidbody = null;
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();    
    }

    public void Shoot(Vector3 dir, int speed, Action<Enemy> action)
    {
        OnHit = action;
        Rigidbody.velocity = dir * speed;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        transform.rotation = lookDir;
    }

    private Action<Enemy> OnHit = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            if (OnHit != null) OnHit(other.GetComponent<Enemy>());

            if (!isAOE) gameObject.SetActive(false);
        }
    }
}