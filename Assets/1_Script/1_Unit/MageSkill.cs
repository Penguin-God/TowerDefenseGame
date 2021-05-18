using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : MonoBehaviour
{
    SphereCollider sphereCollider;
    public TeamSoldier teamSoldier;
    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(PosionAttack());
    }

    IEnumerator PosionAttack()
    {
        yield return new WaitForSeconds(0.2f);
        sphereCollider.enabled = true;
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
}
