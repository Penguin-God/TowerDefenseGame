using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.EnemyStern(100, 5);
            enemy.OnDamage(400000);
        }
    }
}
