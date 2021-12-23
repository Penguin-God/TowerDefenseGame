using UnityEngine;

public class EnemyBoss : NomalEnemy
{
    //private void Awake()
    //{
    //    nomalEnemy = GetComponent<NomalEnemy>();
    //    enemySpawn = GetComponentInParent<EnemySpawn>();
    //    parent = transform.parent.GetComponent<Transform>();
    //    parentRigidbody = GetComponentInParent<Rigidbody>();
    //}

    public override void Dead()
    {
        base.Dead();
        Destroy(parent.gameObject, 1f);
    }
}