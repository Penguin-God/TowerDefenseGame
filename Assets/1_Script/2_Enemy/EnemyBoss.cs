using UnityEngine;

public class EnemyBoss : NomalEnemy
{
    private void Awake()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
        enemySpawn = GetComponentInParent<EnemySpawn>();
        parent = transform.parent.GetComponent<Transform>();
        parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    public override void Dead()
    {
        base.Dead();

        SetDeadVariable();

        Destroy(parent.gameObject);
    }

    void SetDeadVariable()
    {
        parent.gameObject.SetActive(false);
        parent.position = new Vector3(500, 500, 500);
    }
}