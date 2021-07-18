﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : MonoBehaviour
{
    SphereCollider sphereCollider;
    public TeamSoldier teamSoldier;
    public float hitTime; // 콜라이더가 켜지기 전 공격 대기 시간

    private void Awake()
    {
        if(GetComponentInParent<TeamSoldier>() != null) teamSoldier = GetComponentInParent<TeamSoldier>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (teamSoldier.unitColor == TeamSoldier.UnitColor.orange) OrangeMageSkill();
    }

    private void OnEnable()
    {
        if(sphereCollider != null)
            StartCoroutine(ShowEffect_Coroutine(hitTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            MageSkile(enemy);
        }
    }

    void MageSkile(Enemy enemy)
    {
        switch (teamSoldier.unitColor)
        {
            case TeamSoldier.UnitColor.red:
                enemy.EnemyStern(100, 5);
                enemy.OnDamage(400000);
                Destroy(transform.parent.gameObject, 3);
                break;
            case TeamSoldier.UnitColor.blue:
                enemy.EnemySlow(99, 5, true);
                if (teamSoldier.GetComponent<Unit_Mage>().isUltimate) enemy.OnDamage(20000);
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
                break;
        }
    }

    void OrangeMageSkill()
    {
        Enemy enemy = teamSoldier.target.GetComponent<Enemy>();
        int damage = teamSoldier.bossDamage + Mathf.RoundToInt((enemy.currentHp / 100) * 20);
        enemy.OnDamage(damage);
    }

    IEnumerator ShowEffect_Coroutine(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
    }
}