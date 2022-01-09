using UnityEngine;

// 중간보스는 이거를 상속받아서 만들 계획
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