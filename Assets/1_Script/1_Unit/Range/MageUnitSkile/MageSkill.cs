using System.Collections;
using UnityEngine;
using System;

public class MageSkill : MonoBehaviour
{
    [SerializeField] protected SphereCollider sphereCollider;
    public float hitTime; // 콜라이더가 켜지기 전 공격 대기 시간

    // 추적 등에서 필요한 함수
    public virtual void OnSkile(Enemy enemy) { }


    private void OnEnable()
    {
        if (sphereCollider != null) StartCoroutine(Co_OnCollider(hitTime));
    }

    IEnumerator Co_OnCollider(float delayTIme)
    {
        yield return new WaitForSeconds(delayTIme);
        if (sphereCollider != null) sphereCollider.enabled = true;
    }


    private void OnDisable()
    {
        if (sphereCollider != null) sphereCollider.enabled = false;
    }

    // Trigger 충돌 시 돌아가는 가상 함수
    public virtual void HitSkile(Enemy enemy) { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            HitSkile(enemy);
        }
    }

    //void MageSkile(Enemy enemy)
    //{
    //    switch (teamSoldier.unitColor)
    //    {
    //        case TeamSoldier.UnitColor.red:
    //            enemy.EnemyStern(100, 5);
    //            enemy.OnDamage(400000);
    //            Destroy(transform.parent.gameObject, 3);
    //            break;
    //        case TeamSoldier.UnitColor.blue:
    //            enemy.EnemySlow(99, 5, true);
    //            if (teamSoldier.GetComponent<Unit_Mage>().isUltimate) enemy.OnDamage(20000);
    //            break;
    //        case TeamSoldier.UnitColor.yellow:
    //            break;
    //        case TeamSoldier.UnitColor.green:
    //            break;
    //        case TeamSoldier.UnitColor.orange:
    //            break;
    //        case TeamSoldier.UnitColor.violet:
    //            enemy.EnemyPoisonAttack(25, 8, 0.3f, 120000);
    //            break;
    //    }
    //}
}