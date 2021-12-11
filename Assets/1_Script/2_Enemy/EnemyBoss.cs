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
        // normalEenemy에서 풀로 되돌리는 버그가 있어서 얘는 안됨
        // base.Dead();

        OnDeath();
        SetDeadVariable();

        Destroy(parent.gameObject, 1f);
    }

    void SetDeadVariable()
    {
        parent.position = new Vector3(500, 500, 500);
        parent.gameObject.SetActive(false);

        maxSpeed = 0;
        speed = 0;
        isDead = true;
        maxHp = 0;
        currentHp = 0;
        hpSlider.maxValue = 0;
        hpSlider.value = 0;
    }
}