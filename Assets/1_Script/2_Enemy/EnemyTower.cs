using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTower : Enemy
{
    private void Awake()
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack") // 임시
        {
            AttackWeapon attackWeapon = other.GetComponentInParent<AttackWeapon>();
            TeamSoldier teamSoldier = attackWeapon.attackUnit.GetComponent<TeamSoldier>();
            if (teamSoldier.unitType == TeamSoldier.Type.archer) Destroy(other.gameObject); // 아처 공격이면 총알 삭제

            OnDamage(attackWeapon.damage);
        }
    }
}
