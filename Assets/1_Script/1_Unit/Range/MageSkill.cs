﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : MonoBehaviour
{
    SphereCollider sphereCollider;
    public bool moveEffect;
    public TeamSoldier teamSoldier;
    public float hitTime; // 콜라이더가 켜지는 등 공격 타임
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowEffect_Coroutine(hitTime));
        if (moveEffect) 
        {
            StartCoroutine(MeteorWait());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Debug.Log(enemy.transform.gameObject);
            MageSkile(enemy);
        }
        else if(other.tag == "World" && moveEffect)
        {
            MeteotExplosion();
        }
    }

    void MageSkile(Enemy enemy)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                enemy.OnDamage(3000);
                enemy.EnemyStern(100, 5);
                Destroy(gameObject, 1);
                break;
            case TeamSoldier.UnitColor.blue:
                enemy.EnemySlow(99, 5);
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                enemy.EnemyPoisonAttack(25, 8, 0.3f, 2000);
                break;
        }
    }

    IEnumerator ShowEffect_Coroutine(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
    }

    [Header("메테오 전용 변수")]
    [SerializeField]
    private float speed;
    public GameObject explosionObject;

    IEnumerator MeteorWait()
    {
        yield return new WaitUntil(() => teamSoldier != null);
        Vector3 enemyPosition = teamSoldier.target.position + teamSoldier.target.GetComponent<NomalEnemy>().dir.normalized * teamSoldier.target.GetComponent<NomalEnemy>().speed;
        StartCoroutine(ShotMeteor(enemyPosition));
    }

    IEnumerator ShotMeteor(Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(1f);
        Vector3 enemyDirection = (enemyPosition - this.transform.position).normalized;
        Rigidbody rigid = this.GetComponent<Rigidbody>();
        rigid.velocity = enemyDirection * speed;
    }

    public GameObject[] meteors;
    void MeteotExplosion()
    {
        foreach (GameObject meteor in meteors)
            meteor.SetActive(false);
        //this.gameObject.SetActive(false);
        explosionObject.GetComponent<MageSkill>().teamSoldier = this.teamSoldier;
        explosionObject.SetActive(true);
    }
}
