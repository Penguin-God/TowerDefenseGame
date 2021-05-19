using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : MonoBehaviour
{
    SphereCollider sphereCollider;
    public TeamSoldier teamSoldier;
    public float hitTime; // 콜라이더가 켜지는 등 공격 타임
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowEffect_Coroutine(hitTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
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
                break;
            case TeamSoldier.UnitColor.blue:
                enemy.EnemySlow(99);
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                enemy.EnemyPoisonAttack(25, 8, 0.3f);
                break;
        }
    }

    IEnumerator ShowEffect_Coroutine(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
    }
}
