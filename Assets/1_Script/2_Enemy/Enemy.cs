using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // 상태 변수
    public int maxHp;
    public int currentHp;
    public bool isDead;
    public Slider hpSlider;

    protected Rigidbody parentRigidbody;

    // 대미지 관련 함수
    public void OnDamage(int damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
        if (currentHp <= 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {

    }

    NomalEnemy nomalEnemy;
    private void Start()
    {
        nomalEnemy = GetComponent<NomalEnemy>();
    }

    public void EnemyStern(int sternPercent, float sternTime)
    {
        if (this.gameObject.CompareTag("Tower")) return;

        int random = Random.Range(0, 100);
        if (random < sternPercent)
        {
            StopAllCoroutines();
            StartCoroutine(SternCoroutine(sternTime));
        }
    }

    IEnumerator SternCoroutine(float sternTime)
    {
        parentRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sternTime);
        parentRigidbody.velocity = nomalEnemy.dir * nomalEnemy.speed;
    }

    public void EnemySlow(float slowPercent)
    {
        if (this.gameObject.CompareTag("Tower")) return;

        nomalEnemy.speed -= nomalEnemy.speed * (slowPercent / 100);
    }

    public IEnumerator PoisonAttack(int poisonPercent, int poisonCount, float poisonDelay)
    {
        int poisonDamage = Mathf.RoundToInt(currentHp * poisonPercent / 100);
        for (int i = 0; i < poisonCount; i++)
        {
            if (poisonDamage <= 0) poisonDamage = 1; // 독 최소뎀
            if (currentHp > 1) OnDamage(poisonDamage); // 독으로는 못죽임
            yield return new WaitForSeconds(poisonDelay);
        }
    }
}
