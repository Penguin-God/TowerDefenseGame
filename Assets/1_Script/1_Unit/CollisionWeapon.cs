using UnityEngine;
using System;

public class CollisionWeapon : MonoBehaviour
{
    [SerializeField] bool isAOE; // area of effect : 범위(광역) 공격

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public event Action<Enemy> UnitOnDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            if (UnitOnDamage != null) UnitOnDamage(other.GetComponent<Enemy>());

            if (!isAOE) Destroy(gameObject);
        }
    }
}