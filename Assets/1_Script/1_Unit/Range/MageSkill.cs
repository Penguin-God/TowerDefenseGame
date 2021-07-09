using System.Collections;
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

    private void OnEnable()
    {
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
                enemy.OnDamage(15000);
                Destroy(transform.parent.gameObject, 3);
                break;
            case TeamSoldier.UnitColor.blue:
                enemy.EnemySlow(99, 5, true);
                if (teamSoldier.GetComponent<Unit_Mage>().isUltimate) enemy.OnDamage(5000);
                break;
            case TeamSoldier.UnitColor.yellow:
                break;
            case TeamSoldier.UnitColor.green:
                break;
            case TeamSoldier.UnitColor.orange:
                break;
            case TeamSoldier.UnitColor.violet:
                enemy.EnemyPoisonAttack(25, 8, 0.3f, 8000);
                break;
        }
    }

    IEnumerator ShowEffect_Coroutine(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        sphereCollider.enabled = true;
    }
}